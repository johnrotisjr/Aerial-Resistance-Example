using System.Collections.Generic;
using Debug_Module;
using Framework_Module;
using Framework_Module.Core;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.State;
using Framework_Module.Interfaces;

namespace World_Module.WorldStates
{
    public class WorldStateManager : IWorldStateManager
    {
        private readonly Dictionary<WorldStateType, WorldGameState> gameStates;
        public IWorldGameState CurrentState { get; private set; }
        private readonly EventBus eventBus;
        private HashSet<WorldStateType> pausedStates = new();

        public WorldStateManager(IScreenManager screenManager, IConfigDatabase config, IPlayerController playerController, 
            GameObjectPooler objectPooler, IMissionManager missionManagerService, 
            IObjectiveManager objectiveManagerService, IInputController inputController, 
            IViewportBoundsProvider viewportBoundsProvider, IGameData gameData, IAudio audio, EventBus eventBus)
        {
            this.eventBus = eventBus;
            gameStates = new()
            {
                { WorldStateType.Init, new InitWorldState(this) },
                { WorldStateType.Play, new PlayWorldState(screenManager, config, playerController, inputController, eventBus, 
                    objectPooler, missionManagerService, objectiveManagerService, viewportBoundsProvider, gameData, audio, this) },
                { WorldStateType.Pause, new PauseWorldState(inputController, audio, eventBus, this) },
                { WorldStateType.Gameover, new GameoverWorldState(eventBus, this) },
            };
        }
        
        public void Initialize()
        {
            ChangeState(WorldStateType.Init);
        }

        public void Update()
        {
            CurrentState?.Update();
        }

        public void FixedUpdate()
        {
            CurrentState?.FixedUpdate();
        }

        public void ClearPausedStates() => pausedStates.Clear();

        /// <summary>
        /// Change the current world state.
        /// </summary>
        /// <param name="newStateType"></param>
        /// <param name="pauseCurrentState"></param>
        public void ChangeState(WorldStateType newStateType, bool pauseCurrentState = false)
        {
            if (!gameStates.TryGetValue(newStateType, out var state))
            {
                DebugLogger.Log($"Could not find world state of type {newStateType}", LogCategory.World, LogLevel.Error);
                return;
            }
            
            DebugLogger.Log($"Changing game state: {CurrentState} → {newStateType}", LogCategory.Framework, LogLevel.Log);
            
            eventBus.Publish(new WorldStateChangeEvent(CurrentState?.WorldStateType ?? WorldStateType.None, newStateType));

            if (pauseCurrentState)
            {
                if(CurrentState != null)
                    pausedStates.Add(CurrentState.WorldStateType);
            }
            else
            {
                CurrentState?.Exit();
            }

            
            CurrentState = state;

            if (pausedStates.Contains(newStateType))
            {
                pausedStates.Remove(newStateType);
            }
            else
            {
                CurrentState.Enter();
            }
        }
        
        public void Shutdown()
        {
            CurrentState = null;
            gameStates?.Clear();
        }
    }
}