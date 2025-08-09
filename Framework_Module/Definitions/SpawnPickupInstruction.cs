using System;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct SpawnPickupInstruction
    {
        [SerializeField] private PickupType type;
        [SerializeField] private float spawnLocationInDegrees;
        [SerializeField] private float spawnDistance;
        
        public PickupType Type => type;
        public float SpawnLocationInDegrees => spawnLocationInDegrees;
        public float SpawnDistance => spawnDistance;
    }
}