using UnityEngine;
using System.Collections;

namespace DeepForge.Game.Data
{
    public enum RoomType
    {
        TestRoom,
        None
    }

    public class RoomDefinition : ScriptableObject
    {
        public GameObject[] tiles;
        public GameObject[] walls;

    }
}