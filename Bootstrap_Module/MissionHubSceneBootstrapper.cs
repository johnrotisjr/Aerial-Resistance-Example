using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Core;
using Framework_Module.Event;
using Framework_Module.Game_State;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using Input_Module;
using Ui_Module.Screens;
using UnityEngine;
using World_Module;
using World_Module.WorldStates;

namespace Bootstrap_Module
{
    /// <summary>
    /// Initializes gameplay-specific managers, services, and entities at the start of a Mission scene.
    /// Handles scene-local setup.
    /// </summary>
    internal class MissionHubSceneBootstrapper : MonoBehaviour
    {
        [Header("Screens")]
        [SerializeField] private MapScreen mapScreen;
        [SerializeField] private PlaneSelectionScreen planeSelectionScreen;
        [SerializeField] private WeaponSelectionScreen weaponSelectionScreen;

        private readonly List<GameScreen> screenCache = new();
        
        private IScreenManager screenManager;
        private IViewportBoundsProvider viewportBoundsProvider;
        private IMissionManager missionManager;
        private IObjectiveManager objectiveManager;
        private IConfigDatabase configDatabase;
        private IInputController inputController;
        private WorldStateManager worldStateManager;
        private CollisionResolver collisionResolver;
        private InputDispatcher inputDispatcher;
        private GameStateManager gameStateManager;
        private GameObjectPooler gameObjectPooler;
        private EventBus eventBus;

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
        }
        
        
        private void InstantiateScreens()
        {
            weaponSelectionScreen.Inject(configDatabase, screenManager, eventBus);
            screenManager.RegisterScreen(weaponSelectionScreen);
                
            
            mapScreen.GameScreenInject(screenManager, eventBus);
            screenManager.RegisterScreen(mapScreen);
            
            planeSelectionScreen.Inject(configDatabase, screenManager, eventBus);
            screenManager.RegisterScreen(planeSelectionScreen);
            
            screenCache.Add(weaponSelectionScreen);
            screenCache.Add(mapScreen);
            screenCache.Add(planeSelectionScreen);
        }

        private void Awake()
        {
            GetGlobalServices();
        }

        private void Start()
        {
            InstantiateScreens();
        }

        private void OnDestroy()
        {
            foreach (var gameScreen in screenCache)
            {
                if (gameScreen)
                {
                    screenManager.UnregisterScreen(gameScreen);
                    Destroy(gameScreen.gameObject);
                }
            }
            screenCache.Clear();
        }
    }
}