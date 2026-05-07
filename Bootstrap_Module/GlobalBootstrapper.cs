using System;
using System.Runtime.CompilerServices;
using Audio_Module;
using Data_Module;
using Framework_Module.Core;
using Framework_Module.Event;
using Framework_Module.Game_State;
using Framework_Module.GameData.Databases;
using Framework_Module.Interfaces;
using Framework_Module.Scenes;
using Framework_Module.Service;
using Input_Module;
using Ui_Module;
using Ui_Module.Screens;
using UnityEngine;
using UnityEngine.Serialization;
using World_Module;
using World_Module.Vehicle_Controller;

[assembly: InternalsVisibleTo("Game.Tests")]
namespace Bootstrap_Module
{
    /// <summary>
    /// Initializes global systems and services that persist across all scenes.
    /// Sets up core game infrastructure at application start.
    /// </summary>
    internal class GlobalBootstrapper : MonoBehaviour
    {

        [Header("Service MonoBehaviors")]
        [SerializeField] private SceneTransitionController sceneTransitionController;
        [SerializeField] private ScreenManager screenManager;
        [SerializeField] private InputController inputController;
        [SerializeField] private ViewportBoundsProvider viewportBoundsProvider;
        [SerializeField] private ConfigDatabase configDatabase;
        [SerializeField] private GameObjectPooler gameObjectPooler;
        [SerializeField] private Audio audioService;

        private void Awake()
        {
            var eventBus = new EventBus();
            var sceneLoader = new SceneLoader(eventBus);
            var sceneDirector = new SceneDirector();
            var gameData = new GameData(new PersistentPlayerData(configDatabase.GetAllLevelDefinitions().Length), new TransientPlayerData(eventBus));
            var playerController = new PlayerController(gameData, viewportBoundsProvider, eventBus);
            var gameStateManager = new GameStateManager(sceneDirector, eventBus, gameData, inputController, screenManager, audioService);
            var uiInputHandler = new UiInputHandler(gameStateManager, screenManager, sceneLoader);
            var inputDispatcher = new InputDispatcher(uiInputHandler, playerController);
            var inputBindingManager = new InputBindingManager(inputController);

            audioService.Inject(eventBus, gameObjectPooler, new MusicPlayer(), new SfxPlayer(), configDatabase);
            screenManager.Inject(eventBus);
            inputController.Inject(inputDispatcher);
            
            //TODO: Create an adapter to avoid this inject method
            sceneDirector.Inject(sceneLoader, sceneTransitionController, gameStateManager);

            Services.Instance.Register<EventBus>(eventBus);
            Services.Instance.Register<InputDispatcher>(inputDispatcher);
            Services.Instance.Register<InputBindingManager>(inputBindingManager);
            Services.Instance.Register<GameStateManager>(gameStateManager);
            Services.Instance.Register<GameObjectPooler>(gameObjectPooler);
            Services.Instance.Register<SceneDirector>(sceneDirector);
            Services.Instance.Register<SceneLoader>(sceneLoader);
            Services.Instance.Register<IGameData>(gameData);
            Services.Instance.Register<IAudio>(audioService);
            Services.Instance.Register<IInputController>(inputController);
            Services.Instance.Register<IConfigDatabase>(configDatabase);
            Services.Instance.Register<IScreenManager>(screenManager);
            Services.Instance.Register<IViewportBoundsProvider>(viewportBoundsProvider);
            Services.Instance.Register<IPlayerController>(playerController);

            DontDestroyOnLoad(gameObject);
        }

        private void UnregisterServices()
        {
            Services.Instance.Unregister<EventBus>();
            Services.Instance.Unregister<GameData>();
            Services.Instance.Unregister<InputDispatcher>();
            Services.Instance.Unregister<InputBindingManager>();
            Services.Instance.Unregister<GameStateManager>();
            Services.Instance.Unregister<GameObjectPooler>();
            Services.Instance.Unregister<SceneDirector>();
            Services.Instance.Unregister<SceneLoader>();
            Services.Instance.Unregister<IAudio>();
            Services.Instance.Unregister<IInputController>();
            Services.Instance.Unregister<IConfigDatabase>();
            Services.Instance.Unregister<IScreenManager>();
            Services.Instance.Unregister<IViewportBoundsProvider>();
            Services.Instance.Unregister<IPlayerController>();
        }

        private void OnDestroy()
        {
            UnregisterServices();
        }
    }
}
