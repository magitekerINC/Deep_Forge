using UnityEngine;
using System.Collections.Generic;
using DeepForge.Game.Data;
using System;
using System.Collections;
using DeepForge.Utility;

namespace DeepForge.Game
{
    public class Floor : MonoBehaviour
    {

        #region Room Data

        public class RoomData
        {
            private Color color = Color.red;
            public Color Color
            {
                set
                {
                    color = value;
                }
            }
            
            private List<Rect> intersections = new List<Rect>();
            private Rect roomRect;

            private RoomType type;

            public RoomData(RoomType _type, int roomWidth, int roomLength,
                int floorWidth, int floorLength)
            {
                type = _type;


                var sizeVec = SizeRoom(roomWidth, roomLength);
                roomRect = new Rect(0f, 0f, sizeVec.x, sizeVec.y);
                PositionRoom(
                    (int)(floorWidth / 2f),
                    (int)(floorLength / 2f)
                    );
                
            }

            public void PositionRoom(int xDim, int yDim)
            {
                var result = Vector2.zero;

                var roomXPos =
                    Utility.Math.GetRandom(-xDim + 10, (int)(xDim - roomRect.width));
                var roomYPos =
                    Utility.Math.GetRandom(-yDim + 10, (int)(yDim - roomRect.height));

                roomRect.x = roomXPos;
                roomRect.y = roomYPos;

            }

            public Vector2 SizeRoom(int width, int length)
            {
                var size = Vector2.zero;
                var roomWidth = Utility.Math.GetRandom(
                    (int)(width * 0.15f),
                    (int)(width * 0.5f)
                    );

                var roomLength = Utility.Math.GetRandom(
                    (int)(length * 0.15f),
                    (int)(length * 0.5f)
                    );

                size.x = roomWidth;
                size.y = roomLength;
                return size;
            }

            public void MoveRoom(float xDir, float yDir)
            {
                roomRect.x += xDir;
                roomRect.y += yDir;

            }

            public float GetSize()
            {
                return roomRect.size.magnitude;
            }

            public bool isOverlapping(Rect other)
            {
                if (this.roomRect == other)
                    return false;

                return roomRect.Overlaps(other, true);
            }

            public bool CheckOverlap(Rect other)
            {
                if (this.roomRect == other)
                    return false;

                bool result = isOverlapping(other);

                if (result && !intersections.Contains(other))
                {
                    intersections.Add(other);
                }

                return result;
            }

            public void ResolveOverlaps()
            {
                Vector2 disp = Vector2.zero;
                for (int i = intersections.Count - 1; i >= 0; --i)
                {
                    var curr = intersections[i];
                    var delta = (roomRect.center - curr.center);
                    disp += delta.normalized * (roomRect.size.sqrMagnitude / delta.sqrMagnitude);
                    
                }

                MoveRoom(disp.x, disp.y);
                intersections.Clear();
            }

            public bool HasIntersections()
            {
                return intersections.Count != 0;
            }

            public bool Contains(Vector2 pos)
            {
                return roomRect.Contains(pos);
            }


#if UNITY_EDITOR
            public Rect RoomRect
            {
                get { return roomRect; }
            }

            public void DrawGizmo()
            {

                Gizmos.color = color;
                Gizmos.DrawWireCube(
                    roomRect.center,
                    roomRect.size
                    );
            }
#endif
        }

        #endregion 

        #region Fields


        public GameObject tilePrefab;
        public GameObject wallPrefab;

        [SerializeField]
        private int floorWidth = 0;
        [SerializeField]
        private int floorLength = 0;
        private List<RoomData> rooms = new List<RoomData>();
        public int roomCount = 0;
        private Rect floorRect;
        private float floorSize = 0f;
        private Graph<RoomData> floorGraph = new Graph<RoomData>();
        #endregion

        #region Setup
        void Start()
        {
            roomCount = Utility.Math.GetRandom(4, 8);
            
            floorRect = new Rect(
                -floorWidth,
                -floorLength,
                floorWidth * 2f,
                floorLength * 2f
                );

            StartCoroutine(Generate());
           
        }

        public void Reset()
        {
            StopAllCoroutines();
            rooms.Clear();
            StartCoroutine(Generate());
        }

        IEnumerator Generate()
        {
            floorSize = floorRect.size.magnitude;
     
            for (int i = 0; i < roomCount; ++i)
            {
                CreateRoom();
                yield return 0;
            }

            yield return StartCoroutine(ResolveOverlaps());
        }

        IEnumerator ResolveOverlaps()
        {
            bool success = false;

            Queue<RoomData> processQueue = new Queue<RoomData>();
            int cycles = 0;
            while (!success)
            {
                //Resolve Intersections
                while (processQueue.Count > 0)
                {
                    var cur = processQueue.Dequeue();
                    cur.ResolveOverlaps();
                    yield return 0;
                }

                //Discover Intersections;
                for (int i = 0; i < rooms.Count; ++i)
                {
                    for (int j = 0; j < rooms.Count; ++j)
                    {
                        if (i != j)
                        {
                            rooms[i].CheckOverlap(rooms[j].RoomRect);

                        }
                    }

                    if (rooms[i].HasIntersections())
                        processQueue.Enqueue(rooms[i]);
                }

                success = processQueue.Count == 0 || cycles++ == 100;
                yield return 0;
                
            }

            for (int i = rooms.Count - 1; i >= 0; --i)
            {
                if (rooms[i].HasIntersections())
                {
                    rooms[i].Color = Color.blue;
                }
            }
                yield return 0;
            Debug.Log("Done!");
        }

        public void CreateRoom()
        {
            var r = new RoomData(
                RoomType.TestRoom,
                floorWidth,
                floorLength,
                (int)(floorRect.width),
                (int)(floorRect.height)
                );
           
                rooms.Add(r);
            
        }

        public void CreateTile(int xPos, int yPos)
        {
            throw new NotImplementedException("TO DO: Create Tile");
        }

        public void CreateWalls()
        {
            throw new NotImplementedException("TO DO: Create Walls");
        }

        public void EncodeFloor()
        {
            throw new NotImplementedException("TO DO: Encode Floor");
        }

        public bool Contains(Vector2 pos)
        {
            var result = false;

            foreach (RoomData r in rooms)
            {
                result = r.Contains(pos);
            }

            return result;
        }
        #endregion

#if UNITY_EDITOR
        void OnGUI()
        {
            GUILayout.BeginVertical("box");
            if (GUILayout.Button("RESET", GUILayout.Width(250f), GUILayout.Height(50f)))
            {
                Reset();
            }

            GUILayout.BeginHorizontal("box");
            GUILayout.Space(20f);
            GUILayout.Label("Room Count: " + roomCount.ToString(), GUILayout.Width(100f));
            var count = GUILayout.HorizontalSlider(roomCount, 1f, 100f);
            if (count != roomCount)
            {
                roomCount = (int)(count);
                Reset();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            
        }

        void OnDrawGizmos()
        {
            if (rooms.Count == 0)
                return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(
                floorRect.center,
                floorRect.size);

            foreach (RoomData r in rooms)
            {
                r.DrawGizmo();
            }
        }
#endif
    }
}