using Framework_Module.Enums;
using Framework_Module.Core;
using Framework_Module.Extensions;
using Framework_Module.Interfaces;

namespace Framework_Module.Game_State
{
    public class SplashGameState : GameState
    {

        public SplashGameState(SceneDirector sceneDirector, GameStateManager gameStateManager, IInputController inputController) : 
            base(sceneDirector, gameStateManager, inputController)
        {

        }

        public override GameStateType GameStateType => GameStateType.Splash;

        public override void Enter()
        {
            CoroutineRunner.WaitForSeconds(3, OnComplete);
        }

        private async void OnComplete()
        {
            await SceneDirector.Transition(SceneType.Title);
            GameStateManager.ChangeState(GameStateType.Title);
        }

        public override void Exit()
        {
        }
    }
}