using System;
using Framework_Module.Configs.Ai;
using Framework_Module.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct SpawnVehicleInstruction
    {
        [SerializeField] private VehicleArchetype archetype;
        [SerializeField] private AiBehaviorSequenceConfig aiBehaviorSequenceOverride;
        [SerializeField] private float fireRateAdjustment;
        [SerializeField] private float spawnLocationInDegrees;
        [SerializeField] private float spawnDistance;
        
        [FormerlySerializedAs("aiSpawnMovementDefinition")]
        [FormerlySerializedAs("aiSpawnMovementRule")]
        [FormerlySerializedAs("aiSpawnRule")] [FormerlySerializedAs("aiSpawnPositionerData")] 
        [SerializeField] private SpawnMovementInstruction aiSpawnMovementInstruction;
        
        public VehicleArchetype Archetype => archetype;
        public AiBehaviorSequenceConfig AiBehaviorSequenceOverride => aiBehaviorSequenceOverride;
        public float FireRateAdjustment => fireRateAdjustment;
        public float SpawnLocationInDegrees => spawnLocationInDegrees;
        public float SpawnDistance => spawnDistance;
        public SpawnMovementInstruction AiSpawnMovementInstruction => aiSpawnMovementInstruction;
        
        
        public SpawnVehicleInstruction(
            VehicleArchetype archetype,
            AiBehaviorSequenceConfig aiBehaviorSequenceOverride,
            float fireRateAdjustment,
            float spawnLocationInDegrees,
            float spawnDistance,
            SpawnMovementInstruction aiSpawnMovementInstruction)
        {
            this.archetype = archetype;
            this.aiBehaviorSequenceOverride = aiBehaviorSequenceOverride;
            this.fireRateAdjustment = fireRateAdjustment;
            this.spawnLocationInDegrees = spawnLocationInDegrees;
            this.spawnDistance = spawnDistance;
            this.aiSpawnMovementInstruction = aiSpawnMovementInstruction;
        }
    }
}