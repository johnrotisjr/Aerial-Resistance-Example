using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct MissionDefinition
    {
        [SerializeField] private SpawnVehicleInstruction[] spawnVehicleInstructions;
        [SerializeField] private SpawnPickupInstruction[] spawnPickupInstructions;
        [SerializeField] private SpawnVehicleInstruction bossSpawnVehicleInstruction;
        
        [SerializeField] private ObjectiveDefinition[] primaryObjectivesData;
        [SerializeField] private ObjectiveDefinition[] secondaryObjectivesData;
        [SerializeField] private ObjectiveDefinition[] hiddenObjectivesData;
        [SerializeField] string displayName;
        public string DisplayName => displayName;        
        public IReadOnlyCollection<SpawnVehicleInstruction> SpawnVehicleInstructions => spawnVehicleInstructions;
        public IReadOnlyCollection<SpawnPickupInstruction> SpawnPickupInstructions =>  spawnPickupInstructions;
        public SpawnVehicleInstruction BossSpawnVehicleInstruction => bossSpawnVehicleInstruction;
        
        public IReadOnlyCollection<ObjectiveDefinition> PrimaryObjectivesData => primaryObjectivesData;
        public IReadOnlyCollection<ObjectiveDefinition> SecondaryObjectivesData => secondaryObjectivesData;
        public IReadOnlyCollection<ObjectiveDefinition> HiddenObjectivesData => hiddenObjectivesData;

        public MissionDefinition(string displayName, SpawnVehicleInstruction[] spawnVehicleInstructions, 
            SpawnPickupInstruction[] spawnPickupInstructions, SpawnVehicleInstruction bossSpawnVehicleInstruction,
            ObjectiveDefinition[] primaryObjectivesData, ObjectiveDefinition[] secondaryObjectivesData, 
            ObjectiveDefinition[] hiddenObjectivesData)
        {
            this.displayName = displayName;
            this.spawnVehicleInstructions = spawnVehicleInstructions;
            this.spawnPickupInstructions = spawnPickupInstructions;
            this.bossSpawnVehicleInstruction = bossSpawnVehicleInstruction;
            this.primaryObjectivesData = primaryObjectivesData;
            this.secondaryObjectivesData = secondaryObjectivesData;
            this.hiddenObjectivesData = hiddenObjectivesData;
        }
    }
}