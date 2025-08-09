using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Core;
using Framework_Module.Enums;
using Framework_Module.Event.State;
using Framework_Module.Interfaces;
using Framework_Module.Scenes;
using UnityEngine;
using EventBus = Framework_Module.Event.EventBus;

namespace Framework_Module.Game_State
{
    /// <summary>
    /// Manages transitions between different high-level game states and triggers related system changes.
    /// </summary>

    public class GameStateManager : IGameService
    {
        public GameState CurrentState { get; private set; } 
        private readonly IInputController inputController;
        private readonly EventBus eventBus;
        private Dictionary<GameStateType, GameState> gameStates;
        private readonly SceneDirector sceneDirector;
        private readonly IScreenManager manager;
        private readonly IGameData gameData;
        private IAudio audio;
        
        public GameStateManager(SceneDirector sceneDirector, EventBus eventBus, IGameData gameData,
            IInputController inputController, IScreenManager manager, IAudio audio)
        {
            this.eventBus = eventBus;
            this.inputController = inputController;
            this.sceneDirector = sceneDirector;
            this.manager = manager;
            this.gameData = gameData;
            this.audio = audio;
        }

        public void Initialize()
        {
            Time.timeScale = 0;
            gameStates = new()
            {
                { GameStateType.Title, new TitleGameState(sceneDirector, this, inputController, audio) },
                { GameStateType.Menu, new MenuGameState(sceneDirector, this, inputController, eventBus) },
                { GameStateType.Gameplay, new GameplayGameState(eventBus, sceneDirector, this, inputController, audio) },
                { GameStateType.Boot, new BootGameState(sceneDirector, this, inputController, eventBus) },
                { GameStateType.Loading, new LoadingGameState(sceneDirector, this, inputController) },
                { GameStateType.Splash, new SplashGameState(sceneDirector, this, inputController) },
                { GameStateType.MissionHub, new MissionHubGameState(gameData, sceneDirector, this, inputController, eventBus, manager, audio) },
            };
            ChangeState(GameStateType.Boot);
        }

        /// <summary>
        /// Change the current game state.
        /// </summary>
        /// <param name="newStateType"></param>
        public void ChangeState(GameStateType newStateType)
        {
            if (!gameStates.TryGetValue(newStateType, out var state))
            {
                DebugLogger.Log($"Could not find state of type {newStateType}", LogCategory.Framework, LogLevel.Error);
                return;
            }
            
            DebugLogger.Log($"Changing game state: {CurrentState} → {newStateType}", LogCategory.Framework, LogLevel.Log);
            
            eventBus.Publish(new GameStateChangeEvent(CurrentState?.GameStateType ?? GameStateType.None, newStateType));

            CurrentState?.Exit();
            CurrentState = state;
            CurrentState?.Enter();
        }

        public void Shutdown()
        {
            CurrentState = null;
            gameStates?.Clear();
        }
    }
}