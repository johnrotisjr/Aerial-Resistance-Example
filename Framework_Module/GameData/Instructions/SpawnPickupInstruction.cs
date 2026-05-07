using System;
using Framework_Module.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework_Module.GameData.Instructions
{
    [Serializable]
    public struct SpawnPickupInstruction
    {
        [SerializeField] private PickupType type;
        [FormerlySerializedAs("spawnEdge")] [SerializeField] private DirectionType direction;
        [SerializeField][Range(0, 1.0f)] private float spawnEdgePos;
        [SerializeField] private float spawnDistance;
        
        public PickupType Type => type;
        public float SpawnEdgePos => spawnEdgePos;
        public DirectionType DirectionType => direction;
        public float SpawnDistance => spawnDistance;
    }
}