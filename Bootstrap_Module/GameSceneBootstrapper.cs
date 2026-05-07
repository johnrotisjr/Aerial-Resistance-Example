using Debug_Module;
using Framework_Module.Core;
using Framework_Module.Event;
using Framework_Module.Game_State;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using Input_Module;
using Ui_Module.HudObjects;
using Ui_Module.Screens;
using UnityEngine;
using World_Module;
using World_Module.Mission;
using World_Module.Mission.Objective;
using World_Module.WorldStates;

namespace Bootstrap_Module
{
    /// <summary>
    /// Initializes gameplay-specific managers, services, and entities at the start of a gameplay scene.
    /// Handles scene-local setup.
    /// </summary>
    internal class GameSceneBootstrapper : MonoBehaviour
    {
        [SerializeField] private BattleFieldManager battleFieldManager;
        
        [Header("Screens")]
        [SerializeField] private DialogScreen dialogScreen;
        [SerializeField] private Hud hud;
        [SerializeField] private MissionCompleteScreen missionCompleteScreen;
        [SerializeField] private PauseScreen pauseScreen;

        private IPlayerController playerController;
        private IScreenManager screenManager;
        private IViewportBoundsProvider viewportBoundsProvider;
        private IMissionManager missionManager;
        private IObjectiveManager objectiveManager;
        private IConfigDatabase configDatabase;
        private IInputController inputController;
        private WorldObjectSpawner worldObjectSpawner;
        private WorldStateManager worldStateManager;
        private CollisionResolver collisionResolver;
        private InputDispatcher inputDispatcher;
        private GameStateManager gameStateManager;
        private GameObjectPooler gameObjectPooler;
        private EventBus eventBus;
        private IGameData gameData;
        private IAudio audioService;

        private void GetGlobalServices()
        {
            inputDispatcher = Services.Get<InputDispatcher>();
            if (inputDispatcher == null)
            {
                DebugLogger.Log("InputDispatcher is required!", LogCategory.Bootstrap, LogLevel.Error);
                return;
            }

            gameStateManager = Services.Get<GameStateManager>();
            if (gameStateManager == null)
            {
                DebugLogger.Log("GameStateManager is required!", LogCategory.Bootstrap, LogLevel.Error);
                return;
            }

            screenManager = Services.Get<IScreenManager>();
            if (screenManager == null)
            {
                DebugLogger.Log("uiScreenManager is required!", LogCategory.Bootstrap, LogLevel.Error);
                return;
            }

            eventBus = Services.Get<EventBus>();
            if (eventBus == null)
            {
                DebugLogger.Log("eventBus is required!", LogCategory.Bootstrap, LogLevel.Error);
                return;
            }

            viewportBoundsProvider = Services.Get<IViewportBoundsProvider>();
            if (viewportBoundsProvider == null)
            {
                DebugLogger.Log("cameraController is required!", LogCategory.Bootstrap, LogLevel.Error);
                return;
            }
            
            configDatabase = Services.Get<IConfigDatabase>();
            if (configDatabase == null)
            {
                DebugLogger.Log("configDatabase is required!", LogCategory.Bootstrap, LogLevel.Error);
                return;
            }
            
            gameObjectPooler = Services.Get<GameObjectPooler>();
            if (gameObjectPooler == null)
            {
                DebugLogger.Log("gameObjectPooler is required!", LogCategory.Bootstrap, LogLevel.Error);
                return;
            }
            
            inputController = Services.Get<IInputController>();
            if (inputController == null)
            {
                DebugLogger.Log("inputController is required!", LogCategory.Bootstrap, LogLevel.Error);
                return;
            }

            gameData = Services.Get<IGameData>();
            if (gameData == null)
            {
                DebugLogger.Log("TemporaryGameplayData is required!", LogCategory.Bootstrap, LogLevel.Error);
                return;
            }
            
            audioService = Services.Get<IAudio>();
            if (audioService == null)
            {
                DebugLogger.Log("audio is required!", LogCategory.Bootstrap, LogLevel.Error);
                return;
            }
            
            playerController = Services.Get<IPlayerController>();
            if (playerController == null)
            {
                DebugLogger.Log("playerController is required!", LogCategory.Bootstrap, LogLevel.Error);
                return;
            }
        }
        
        private void InitializeNewServices()
        {
            Services.Instance.Register<IBattleFieldManager>(battleFieldManager);
            hud.Inject(eventBus, playerController);
            Services.Instance.Register<IHud>(hud);
            
            objectiveManager = new ObjectiveManager(configDatabase, eventBus);
            Services.Instance.Register<IObjectiveManager>(objectiveManager);
            
            missionManager = new MissionManager(configDatabase, viewportBoundsProvider, objectiveManager, eventBus, gameData, new MovementAiBehaviorFactory(), new AttackAiBehaviorFactory());
            Services.Instance.Register<IMissionManager>(missionManager);

            collisionResolver = new CollisionResolver(viewportBoundsProvider, playerController);
            Services.Instance.Register<CollisionResolver>(collisionResolver);
            
            worldStateManager = new WorldStateManager(screenManager, configDatabase, playerController, gameObjectPooler, 
                missionManager, objectiveManager, inputController, viewportBoundsProvider, gameData, audioService, eventBus);
            Services.Instance.Register<IWorldStateManager>(worldStateManager);
            
            worldObjectSpawner = new(configDatabase, gameObjectPooler, viewportBoundsProvider, new WeaponAiBehaviorFactory(), eventBus);
            Services.Instance.Register<IWorldObjectSpawner>(worldObjectSpawner);
            
            //TODO: Create adapters to avoid these inject methods
            battleFieldManager.Inject(viewportBoundsProvider, worldStateManager);
            missionManager.Inject(configDatabase, worldObjectSpawner);
        }
        
        private void InstantiateScreens()
        {
            missionCompleteScreen.Inject(screenManager, objectiveManager, eventBus, configDatabase, gameData);
            screenManager.RegisterScreen(missionCompleteScreen);

            pauseScreen.Inject(screenManager, objectiveManager, eventBus, configDatabase, gameData);
            screenManager.RegisterScreen(pauseScreen);
            
            dialogScreen.GameScreenInject(screenManager, eventBus);
            screenManager.RegisterScreen(dialogScreen);
        }

        private void UnregisterScreens()
        {
            screenManager.UnregisterScreen(missionCompleteScreen);
            screenManager.UnregisterScreen(pauseScreen);
            screenManager.UnregisterScreen(dialogScreen);
        }

        private void Awake()
        {
            GetGlobalServices();
            InitializeNewServices();
        }

        private void Start()
        {
            InstantiateScreens();
        }

        private void OnDestroy()
        {
            UnregisterScreens();
 
            if(battleFieldManager)
                Destroy(battleFieldManager.gameObject);
            if(hud)
                Destroy(hud.gameObject);
            
            Services.Instance.Unregister<IBattleFieldManager>();
            Services.Instance.Unregister<IMissionManager>();
            Services.Instance.Unregister<IObjectiveManager>();
            Services.Instance.Unregister<CollisionResolver>();
            Services.Instance.Unregister<IWorldStateManager>();
        }
    }
}