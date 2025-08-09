using Framework_Module.Core;
using Framework_Module.Enums;
using Framework_Module.Interfaces;
 

namespace Framework_Module.Game_State
{
    public abstract class GameState : IGameState
    {
        protected readonly SceneDirector SceneDirector;
        protected readonly GameStateManager GameStateManager;
        protected readonly IInputController InputController;
        public abstract GameStateType GameStateType { get; }
        public abstract void Enter();
        public abstract void Exit();

        protected GameState(SceneDirector sceneDirector, GameStateManager gameStateManager, IInputController inputController)
        {
            SceneDirector = sceneDirector;
            GameStateManager = gameStateManager;
            InputController = inputController;
        }
    }
}