using UnityEngine;
using System.Collections;

namespace DeepForge.Utility
{
    public class Math
    {

        public static float EuclidianDistance(Vector2 from, Vector2 to)
        {
            return (to - from).sqrMagnitude;
        }

        public static int ManhattenDistance(Vector2 from, Vector2 to)
        {
            if (from == to)
                return 0;

            int result = 0;
            int deltaX = Mathf.FloorToInt(
                Mathf.Abs(to.x - from.x));
            int deltaY = Mathf.FloorToInt(
                Mathf.Abs(to.y - from.y));

            result = deltaX + deltaY;

            return result;
        }

        public static int GetRandom(int min, int max)
        {
            if (min == max)
                return min;

            return Random.Range(min, max);
        }
 
        public static int GetArrayPos(int x, int y, int width, int height)
        {
            if (width == 0 || height == 0)
                return 0;

            return y * width + x;
        }

        public static int GetArrayPos(Vector2 vPos, int width, int height)
        {
            if (width == 0 || height == 0)
                return 0;

            return GetArrayPos((int)(vPos.x), (int)(vPos.y), width, height);
        }

        public static Vector2 GetPosFromIndex(int index, int width, int height)
        {
            if (width == 0 || height == 0)
                return Vector2.zero;

            return new Vector2(
                Mathf.Floor(index % width),
                Mathf.Floor(index / width)
                );
        }
    }
}