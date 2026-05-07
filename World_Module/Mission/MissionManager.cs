using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Core;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.GameData.Instructions;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;
using World_Module.Vehicle_Controller;
using World_Module.WorldStates;

namespace World_Module.Mission
{
    /// <summary>
    /// Manages the flow of a mission including enemy and boss spawns.
    /// Tracks mission progress and coordinates start/reset behavior.
    /// </summary>
    public class MissionManager : IMissionManager
    {
        private Queue<SpawnVehicleInstruction> spawnVehicleInstructionQueue;
        private Queue<SpawnPickupInstruction> spawnPickupInstructionQueue;
        private SpawnVehicleInstruction bossVehicleInstruction;
        private IWorldObjectSpawner worldObjectSpawner;
        private readonly IObjectiveManager objectiveManager;
        private IWeaponBehaviorFactory weaponBehaviorFactory;
        private readonly IViewportBoundsProvider viewportBoundsProvider;
        private readonly IConfigDatabase configDatabase;
        private MissionDefinition missionDefinition;
        private readonly IGameData gameData;
        private readonly EventBus eventBus;
        private readonly IMovementAiBehaviorFactory movementAiBehaviorFactory;
        private readonly IAttackAiBehaviorFactory attackAiBehaviorFactory;
        
#if UNITY_EDITOR
        private int spawnCount = 0;
#endif
        
        public bool HasCompletedPrimaryObjectives()
        {
            foreach (var objectiveState in objectiveManager.Primary)
            {
                if (!objectiveState.IsComplete)
                    return false;
            }

            return true;
        }

        public MissionManager(IConfigDatabase configDatabase, IViewportBoundsProvider viewportBoundsProvider, IObjectiveManager objectiveManagerService, 
            EventBus eventBusService, IGameData gameData, IMovementAiBehaviorFactory movementAiBehaviorFactory, IAttackAiBehaviorFactory attackAiBehaviorFactory)
        {
            this.configDatabase = configDatabase;
            this.viewportBoundsProvider = viewportBoundsProvider;
            objectiveManager = objectiveManagerService;
            eventBus = eventBusService;
            this.gameData = gameData;
            this.attackAiBehaviorFactory = attackAiBehaviorFactory;
            this.movementAiBehaviorFactory = movementAiBehaviorFactory;
        }

        public void Despawn<T>(string key, T value) where T : IWorldObject, IClearable
        {
            worldObjectSpawner.Despawn(key, value);
        }

        public void Inject(IConfigDatabase database, IWorldObjectSpawner spawner)
        {
            this.worldObjectSpawner = spawner;
            missionDefinition = database.GetMissionDefinition(gameData.TransientPlayerData.SelectedMissionIndex);
            bossVehicleInstruction = missionDefinition.BossSpawnVehicleInstruction;
            spawnVehicleInstructionQueue = new Queue<SpawnVehicleInstruction>(missionDefinition.SpawnVehicleInstructions);
            spawnPickupInstructionQueue = new Queue<SpawnPickupInstruction>(missionDefinition.SpawnPickupInstructions);
        }

        public IVehicle SpawnPlayer()
        {
            if (gameData.TransientPlayerData.SelectedVehicleId == null)
            {
                DebugLogger.Log("Selected plane must be set!", LogCategory.World, LogLevel.Error);
                return null;
            }

            configDatabase.GetVehicleData(gameData.TransientPlayerData.SelectedVehicleId, out var config);
            var instruction = new SpawnVehicleInstruction(
                config,
                null,
                0,
                DirectionType.Left,
                .5f,
                0,
                new SpawnMovementInstruction(false)
            );
            
            return worldObjectSpawner.Spawn(instruction, false);
        }

        public IWeapon SpawnWeapon(WeaponType weaponType, Vector2 position, Vector2 velocity, Quaternion rotation, AlignmentType alignment, IVehicle parentVehicle)
        {
            var type = weaponType == WeaponType.HomingMissile ? AudioSfxType.MissileLaunch : AudioSfxType.BulletLaunch;
            eventBus.Publish(new PlaySfxEvent(type));
            return worldObjectSpawner.Spawn(weaponType, position, velocity, rotation, alignment, parentVehicle);
        }

        public void Initialize()
        {

        }

        public void Shutdown()
        {
            
        }

        public void Reset()
        { 
#if UNITY_EDITOR
            spawnCount = 0;
#endif

            spawnVehicleInstructionQueue.Clear();
            spawnPickupInstructionQueue.Clear();
            foreach (var i in missionDefinition.SpawnVehicleInstructions)
            {
                spawnVehicleInstructionQueue.Enqueue(i);
            }
            foreach (var i in missionDefinition.SpawnPickupInstructions)
            {
                spawnPickupInstructionQueue.Enqueue(i);
            }
            objectiveManager.ResetData(gameData.TransientPlayerData.SelectedMissionIndex);
        }
        
        private IVehicleController SpawnFoe(SpawnVehicleInstruction vehicleInstruction)
        {
#if UNITY_EDITOR
            DebugLogger.Log($"Spawn num: {spawnCount++} - {vehicleInstruction.VehicleData.ReadableId}", LogCategory.World, LogLevel.Log);
#endif

            var vehicle = worldObjectSpawner.Spawn(vehicleInstruction);
            vehicle.AlignmentType = AlignmentType.Foe;
            var controller = new AiController();
            controller.AssignNewVehicle(vehicle);
            var sequence = vehicleInstruction.AiBehaviorSequenceOverride ? vehicleInstruction.AiBehaviorSequenceOverride : vehicle.VehicleData.VehicleDefinition.AIBehaviorSequence;
            controller.InitializeAi(sequence, vehicleInstruction.AiSpawnMovementInstruction, viewportBoundsProvider, movementAiBehaviorFactory, attackAiBehaviorFactory);
            return controller;
        }

        private IPickup SpawnPickup(SpawnPickupInstruction pickupInstruction)
        { 
            IPickup pickup = worldObjectSpawner.Spawn(pickupInstruction);
            return pickup;
        }

        public List<IVehicleController> CheckEnemySpawn(float totalDistanceFlown)
        {
            var spawnedVehicleControllers = new List<IVehicleController>();
            while (spawnVehicleInstructionQueue.TryPeek(out var i) && i.SpawnDistance < totalDistanceFlown)
            {
                var instruction = spawnVehicleInstructionQueue.Dequeue();
                var vehicle = SpawnFoe(instruction);
                spawnedVehicleControllers.Add(vehicle);
            }
            return spawnedVehicleControllers;
        }
        
        public List<IPickup> CheckPickupSpawn(float totalDistanceFlown)
        {
            var pickups = new List<IPickup>();
            while (spawnPickupInstructionQueue.TryPeek(out var i) && i.SpawnDistance < totalDistanceFlown)
            {
                var instruction = spawnPickupInstructionQueue.Dequeue();
                var pickup = SpawnPickup(instruction);
                pickups.Add(pickup);
            }
            return pickups;
        }
        
        public IVehicleController CheckBossSpawn(float totalDistanceFlown)
        {
            IVehicleController vehicleController = null;
            if (bossVehicleInstruction.SpawnDistance < totalDistanceFlown)
            {
                var instruction = bossVehicleInstruction;
                vehicleController = SpawnFoe(instruction);
            }
            return vehicleController;
        }
    }
}
