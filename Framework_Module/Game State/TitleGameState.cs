using System.Collections;
using System.Threading.Tasks;
using Framework_Module.Core;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Framework_Module.Game_State
{
    public class TitleGameState : GameState
    {
        public override GameStateType GameStateType => GameStateType.Title;
        private Coroutine waitRoutine;
        private readonly IInputController inputController;
        private readonly IAudio audio;
        
        public TitleGameState(SceneDirector sceneDirector, GameStateManager gameStateManager, 
            IInputController inputController, IAudio audio) : base(sceneDirector, gameStateManager, inputController)
        {
            this.inputController = inputController;
            this.audio = audio;
        }
        
        public override void Enter()
        {
            inputController.EnableUIControls();
            waitRoutine = CoroutineRunner.Begin(AutoTransition());
            audio.MusicPlayer.Start(AudioMusicType.MenuMusic);
        }

        private IEnumerator AutoTransition()
        {
            yield return new WaitForSeconds(5f);
            yield return Transition();
        }

        private async Task Transition()
        {
            await SceneDirector.Transition(SceneType.Menu);
            GameStateManager.ChangeState(GameStateType.Menu);
        }

        public override async void Exit()
        {
            inputController.DisableUIControls();
            CoroutineRunner.End(waitRoutine);
            await Transition();
        }
    }
}