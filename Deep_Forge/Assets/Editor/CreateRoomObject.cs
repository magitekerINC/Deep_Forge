using UnityEngine;
using UnityEditor;
using System.Collections;
using DeepForge.Game.Data;

namespace DeepForge.Editor
{
    public class CreateRoomObject
    {
        [MenuItem("Assets/Create/Room")]
        public static void CreateAsset()
        {
            CreateScriptableObject.CreateAsset<RoomDefinition>();
        }
    }
}