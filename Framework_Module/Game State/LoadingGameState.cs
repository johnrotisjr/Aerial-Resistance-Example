
using Framework_Module.Core;
using Framework_Module.Enums;
using Framework_Module.Interfaces;

namespace Framework_Module.Game_State
{
    public class LoadingGameState : GameState
    {
        public override GameStateType GameStateType => GameStateType.Loading;
        
        public LoadingGameState(SceneDirector sceneDirector, GameStateManager gameStateManager, IInputController inputController) 
            : base(sceneDirector, gameStateManager, inputController)
        {
        }
        
        public override void Enter()
        {

        }

        public override void Exit()
        {
        }
    }
}