using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Core;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.Combat;
using Framework_Module.Event.Gameplay;
using Framework_Module.Event.State;
using Framework_Module.Event.Ui;
using Framework_Module.Extensions;
using Framework_Module.Interfaces;
using UnityEngine;
using World_Module.WorldObjects;
using EventBus = Framework_Module.Event.EventBus;

namespace World_Module.WorldStates
{
    public class PlayWorldState : WorldGameState
    {
        public override WorldStateType WorldStateType => WorldStateType.Play;
 
        private readonly List<IVehicleController> enemyVehicleControllers = new();
        private readonly List<IPickup> activePickups = new();
        private readonly List<IWeapon> weapons = new();

        private readonly IPlayerController playerController;
        private readonly IInputController inputController;
        private readonly IViewportBoundsProvider viewportBoundsProvider;
        private readonly EventBus eventBus;

        private readonly IMissionManager missionManager;
        private IVehicleController boss;

        private readonly GameObjectPooler pooler;
 
        private bool HasCompletedPrimaryObjectives => missionManager.HasCompletedPrimaryObjectives();
        private readonly IObjectiveManager objectiveManager;
        private float totalDistanceFlown = 0f;
        private bool transitioningToGameover;
        private bool bossSpawned = false;
        private readonly IConfigDatabase configDatabase;
        private readonly IGameData gameData;
        private readonly IAudio audio;
        private IScreenManager screenManager;
        private RewardHandler rewardHandler;
        private bool isDialogInProgress;
        
        public PlayWorldState(IScreenManager screenManager, IConfigDatabase config, IPlayerController playerController, IInputController input,
            EventBus bus, GameObjectPooler objectPooler, IMissionManager missionManagerService, IObjectiveManager objectiveManagerService,
            IViewportBoundsProvider viewportBoundsProvider, IGameData gameData, IAudio audio, WorldStateManager worldStateManager) 
            : base(worldStateManager)
        {
            this.screenManager = screenManager;
            this.viewportBoundsProvider = viewportBoundsProvider;
            configDatabase = config;
            this.audio = audio;
            this.playerController = playerController;
            eventBus = bus;
            inputController = input;
            pooler = objectPooler;
            missionManager = missionManagerService;
            objectiveManager = objectiveManagerService;
            WorldStateManager = worldStateManager;
            this.gameData = gameData;
        }
        
        public override void Enter()
        {
            eventBus.Subscribe<QueueSpawnWeaponEvent>(OnQueueSpawnWeapon);
            eventBus.Subscribe<WorldObjectKilledEvent>(OnWorldObjectDestroyed);
            eventBus.Subscribe<PickupCollectedEvent>(OnPickupCollected);
            eventBus.Subscribe<PauseEvent>(OnPause);
            eventBus.Subscribe<HideScreenEvent>(OnHideScreen);
            rewardHandler = new RewardHandler(playerController, configDatabase, eventBus, gameData);
            
            ResetState();
            RegisterPoolObjects();
            SpawnPlayer();
            inputController.EnablePlayerControls();
            eventBus.Publish(new UpdateHudEvent());
            if (configDatabase.GetDialogSequenceDefinition(gameData.TransientPlayerData.SelectedMissionIndex, WorldStateManager.CurrentState.WorldStateType) != null)
            {
                isDialogInProgress = true;
                inputController.EnableUIControls();
                inputController.DisablePlayerControls();
                
                screenManager.OpenScreen(ScreenType.Dialog);
            }
        }

        private void OnHideScreen(HideScreenEvent e)
        {
            if (e.GameScreen.ScreenType == ScreenType.Dialog)
            {
                inputController.DisableUIControls();
                inputController.EnablePlayerControls();
                isDialogInProgress = false;
            }
        }

        private void OnPause(PauseEvent e)
        {
            WorldStateManager.ChangeState(WorldStateType.Pause, true);
        }

        private void ResetState()
        {
            totalDistanceFlown = 0;
            transitioningToGameover = false;
            bossSpawned = false;
            
            for (var i = enemyVehicleControllers.Count-1; i >= 0; i--)
            {
                RemoveFromGame(enemyVehicleControllers[i]);
            }
            enemyVehicleControllers.Clear();
            
            for (var i = activePickups.Count-1; i >= 0; i--)
            { 
                RemoveFromGame(activePickups[i]);
            }
            activePickups.Clear();
            
            for (var i = weapons.Count-1; i >= 0; i--)
            {
                RemoveFromGame(weapons[i]);
            }
            weapons.Clear();

            RemoveFromGame(playerController);
                        
            eventBus.Publish(new UpdateHudEvent());
            missionManager.Reset();
        }

        public override void Exit()
        {
            audio.MusicPlayer.Stop();
            
            eventBus.Unsubscribe<WorldObjectKilledEvent>(OnWorldObjectDestroyed);
            eventBus.Unsubscribe<PickupCollectedEvent>(OnPickupCollected);
            eventBus.Unsubscribe<QueueSpawnWeaponEvent>(OnQueueSpawnWeapon);
            eventBus.Unsubscribe<PauseEvent>(OnPause);
            eventBus.Unsubscribe<HideScreenEvent>(OnHideScreen);
            
            ResetState();
            
            rewardHandler.Dispose();
            inputController.DisablePlayerControls();
        }

        public override void Update()
        {
            playerController.Update();
            
            foreach (var enemyVehicleController in enemyVehicleControllers)
            {
                enemyVehicleController.Update();
            }
            
            if (isDialogInProgress)
                return;
            
            SpawnEnemies();
            SpawnPickups();

            RemoveInvalidEntities();

            CheckForGameOver();

            if (playerController.ControlledVehicle != null)
            {
                var distanceTravelled = Time.deltaTime * playerController.ControlledVehicle.Speed;
                totalDistanceFlown += distanceTravelled;
                objectiveManager.UpdateDistanceTraveled(distanceTravelled);
            }
        }

        private void GameOver()
        {
            eventBus.Publish(new GameOverEvent(HasCompletedPrimaryObjectives));
 
            if(HasCompletedPrimaryObjectives)
            {
                var persistData = gameData.PersistentPlayerData;
                var missionIndex = gameData.TransientPlayerData.SelectedMissionIndex;
                persistData.CompleteLevel(missionIndex);
            }
            
            WorldStateManager.ChangeState(WorldStateType.Gameover, false);
            transitioningToGameover = false;
        }

        private void RegisterPoolObjects()
        {
            foreach (var vehicleData in configDatabase.GetAllVehicleDefinition())
            {
                if (!pooler.IsRegistered(vehicleData.Guid))
                {
                    pooler.Register(vehicleData.Guid, vehicleData.VehicleDefinition.Prefab, 5, 10);
                }   
            }
            
            foreach (var weaponData in configDatabase.GetAllWeaponData())
            {
                if (!pooler.IsRegistered(weaponData.Type.ToString()))
                {
                    pooler.Register(weaponData.Type.ToString(), weaponData.Prefab, 10, 50);
                }   
            }

            if (!pooler.IsRegistered(PrefabKey.Pickup.ToString()))
            {
                pooler.Register(PrefabKey.Pickup.ToString(), configDatabase.PickupPrefab, 10, 100);
            }
        }

        private void SpawnPlayer()
        {
            var newVehicle = missionManager.SpawnPlayer();
            playerController.AssignNewVehicle(newVehicle);
            SetUpgrades(newVehicle);
            newVehicle.AlignmentType = AlignmentType.Friend;   
        }
        
        
        private void SetUpgrades(IVehicle vehicle)
        {
            if (vehicle.AlignmentType == AlignmentType.Friend)
            {
                var vehicleUpgrades = gameData.PersistentPlayerData.GetVehicleUpgrades(vehicle.VehicleData.Guid);
                if (vehicleUpgrades != null)
                {
                    var armorUpgrades = vehicleUpgrades.GetVehicleUpgradeData(UpgradeType.Armor);
                    vehicle.SetArmorUpgradeLevel(armorUpgrades.Level);
                }
            }
        }

        public IVehicle GetClosestEnemy(IWorldObject worldObject)
        {
            float? distSquared = null;
            IVehicle target = null;
            foreach (var controller in enemyVehicleControllers)
            {
                var direction = controller.ControlledVehicle.Position - worldObject.Position;
                var sqrMag = direction.sqrMagnitude;
                if (distSquared == null || sqrMag < distSquared)
                {
                    distSquared = sqrMag;
                    target = controller.ControlledVehicle;
                }
            }

            return target;
        }
        
        public override void FixedUpdate()
        {
            playerController?.FixedUpdate();

            foreach (var vehicleController in enemyVehicleControllers)
            {
                vehicleController?.FixedUpdate();
            }
        }
        
        private void OnQueueSpawnWeapon(QueueSpawnWeaponEvent e)
        {
            configDatabase.GetWeaponDefinition(e.WeaponType, out var weaponData);
            var weapon = missionManager.SpawnWeapon(e.WeaponType, e.Position, e.Direction * weaponData.Speed, e.Rotation, e.AlignmentType, e.ParentVehicle);

            if (weapon == null)
            {
                DebugLogger.Log($"Failed to spawn weapon of type {e.WeaponType.ToString()}.", LogCategory.World, LogLevel.Warning);
                return;
            }
            
            weapons.Add(weapon);
            eventBus.Publish(new WeaponSpawnedEvent(weapon, e.ParentVehicle, e.AlignmentType));
        }

        bool IsWorldObjectOutOfBounds(Vector2 position, Vector2 spriteBounds, Rect bounds)
        {
            var offset = spriteBounds / 2;
            var spriteRect = new Rect(
                position - offset,
                spriteBounds
            );
    
            return !bounds.Overlaps(spriteRect);
        }

        private void SpawnPickups()
        {
            var pickups = missionManager.CheckPickupSpawn(totalDistanceFlown);
            if (pickups.Count > 0)
            {
                activePickups.AddRange(pickups);
            }
        }

        private void SpawnEnemies()
        {
            var vehicleControllers = missionManager.CheckEnemySpawn(totalDistanceFlown);
            if (vehicleControllers.Count > 0)
            {
                foreach (var vehicleController in vehicleControllers)
                {
                    enemyVehicleControllers.Add(vehicleController);
                }
            }

            if (!bossSpawned)
            {
                var newBossController = missionManager.CheckBossSpawn(totalDistanceFlown);
                if (newBossController != null)
                {
                    bossSpawned = true;
                    boss = newBossController;
                    enemyVehicleControllers.Add(newBossController);   
                }
            }
        }

        private void OnPickupCollected(PickupCollectedEvent e)
        {
            RemoveFromGame(e.Pickup);
        }

        private void OnWorldObjectDestroyed(WorldObjectKilledEvent e)
        {
            if (e.WorldObject == boss?.ControlledVehicle) 
                eventBus.Publish(new BossDestroyedEvent(e.WorldObject));
        }
        
        private bool RemoveFromGame(IPlayerController controller)
        {
            if (controller.ControlledVehicle is Vehicle v)
            {
                missionManager.Despawn(v.VehicleData.Guid, v);
                playerController.UnassignVehicle();
                return true;
            }

            return false;
        }

        private bool RemoveFromGame(IVehicleController vehicleController)
        {
            var removed = enemyVehicleControllers.Remove(vehicleController);
            if (removed && vehicleController.ControlledVehicle is Vehicle v)
            {
                missionManager.Despawn(v.VehicleData.Guid, v);
                vehicleController.UnassignVehicle();   
            }

            return removed;
        }
        
        private bool RemoveFromGame(IWeapon weapon)
        {
            var removed = weapons.Remove(weapon);
            if (removed && weapon is Weapon w)
            {
                missionManager.Despawn(w.WeaponType.ToString(), w);
            }
            return removed;
        }

        private bool RemoveFromGame(IPickup pickup)
        {
            var removed = activePickups.Remove(pickup);
            if (removed && pickup is Pickup p)
            {
                missionManager.Despawn(PrefabKey.Pickup.ToString(), p);
            }
            return removed;
        }

        private void RemoveInvalidEntities()
        {
            var viewportRect = viewportBoundsProvider.GetPixelAlignedViewport();
            for (int i = weapons.Count - 1; i >= 0; i--)
            {
                var weapon = weapons[i];
                if(weapon == null)
                    continue;
                
                var isOutOfBounds = IsWorldObjectOutOfBounds(weapon.Position, weapon.SpriteBounds.size, viewportRect);
                if(weapon.IsMarkedDestroyed || isOutOfBounds)
                {
                    RemoveFromGame(weapon);
                    i--;
                }
            }

            var deathBoundary = viewportBoundsProvider.GetDeathBoundaryRect();
            
            for (var i = activePickups.Count - 1; i >= 0; i--)
            {
                var pickup = activePickups[i];
                if (pickup == null)
                    continue;
                
                var isOutOfBounds = IsWorldObjectOutOfBounds(pickup.Position, pickup.SpriteBounds.size, deathBoundary);
                if (isOutOfBounds)
                {
                    RemoveFromGame(pickup);
                    i--;
                }
            }

            for (int i = enemyVehicleControllers.Count - 1; i >= 0; i-- )
            {
                var enemyVehicle = enemyVehicleControllers[i].ControlledVehicle;
                if (enemyVehicle == null || enemyVehicle == boss?.ControlledVehicle) 
                    continue;
                
                bool isOutOfBounds = IsWorldObjectOutOfBounds(enemyVehicle.Position, enemyVehicle.SpriteBounds.size, deathBoundary);
                if (enemyVehicle.IsReadyForRemoval || isOutOfBounds)
                {
                    RemoveFromGame(enemyVehicleControllers[i]);
                    i--;
                }
            }
        }

        private async void CheckForGameOver()
        {
            if (playerController.ControlledVehicle is not Vehicle a)
                return;
            
            if (!transitioningToGameover && (!playerController.ControlledVehicle.IsAlive || HasCompletedPrimaryObjectives))
            {
                transitioningToGameover = true;
                playerController.ControlledVehicle.SetVelocity(Vector2.zero);
                if (playerController.ControlledVehicle.IsReadyForRemoval)
                {
                    RemoveFromGame(playerController);
                }

                inputController.DisablePlayerControls();
                
                await CoroutineRunner.WaitForSecondsAsync(2, GameOver);
            }
        }
    }
}