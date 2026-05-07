using System;
using Framework_Module.Enums;
using Framework_Module.GameData.Ai;
using Framework_Module.GameData.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework_Module.GameData.Instructions
{
    [Serializable]
    public struct SpawnVehicleInstruction
    {
        [SerializeField] private VehicleData vehicleData;
        [SerializeField] private AiBehaviorSequenceConfig aiBehaviorSequenceOverride;

        [FormerlySerializedAs("spawnEdge")] [SerializeField] private DirectionType direction;
        [SerializeField][Range(0, 1.0f)] private float spawnEdgePos;
        [SerializeField] private float spawnDistance;
        [SerializeField] private SpawnMovementInstruction aiSpawnMovementInstruction;
        [TextArea][SerializeField] private string notes;
        
        public VehicleData VehicleData => vehicleData;
        public AiBehaviorSequenceConfig AiBehaviorSequenceOverride => aiBehaviorSequenceOverride;
        public float SpawnEdgePos => spawnEdgePos;
        public DirectionType DirectionType => direction;
        public float SpawnDistance => spawnDistance;
        public SpawnMovementInstruction AiSpawnMovementInstruction => aiSpawnMovementInstruction;

        public SpawnVehicleInstruction(
            VehicleData vehicleData,
            AiBehaviorSequenceConfig aiBehaviorSequenceOverride,
            float fireRateAdjustment,
            DirectionType direction,
            float spawnEdgePos,
            float spawnDistance,
            SpawnMovementInstruction aiSpawnMovementInstruction)
        {
            this.vehicleData = vehicleData;
            this.aiBehaviorSequenceOverride = aiBehaviorSequenceOverride;
            this.spawnEdgePos = spawnEdgePos;
            this.direction = direction;
            this.spawnDistance = spawnDistance;
            this.aiSpawnMovementInstruction = aiSpawnMovementInstruction;
            this.notes = "";
        }
    }
}