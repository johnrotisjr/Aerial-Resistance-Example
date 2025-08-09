using Framework_Module.Core;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.Gameplay;
using Framework_Module.Interfaces;
using UnityEngine.SceneManagement;

namespace Framework_Module.Game_State
{
    public class GameplayGameState : GameState
    {
        public override GameStateType GameStateType => GameStateType.Gameplay;
        private readonly EventBus eventBus;
        private readonly IAudio audio;
        
        public GameplayGameState(EventBus eventBus, SceneDirector sceneDirector, GameStateManager gameStateManager, 
            IInputController inputController, IAudio audio) 
            : base(sceneDirector, gameStateManager, inputController)
        {
            this.eventBus = eventBus;
            this.audio = audio;
        }
 
        public override void Enter()
        {
            eventBus.Subscribe<ExitMissionEvent>(OnExitMission);
            audio.MusicPlayer.Start(AudioMusicType.GameMusic, true, 1);
        }

        public override void Exit()
        {
            eventBus.Unsubscribe<ExitMissionEvent>(OnExitMission);
            audio.MusicPlayer.Stop(1);
        }

        private async void OnExitMission(ExitMissionEvent e)
        {
            await SceneDirector.Transition(SceneType.MissionHub, LoadSceneMode.Single);
            GameStateManager.ChangeState(GameStateType.MissionHub);
        }
    }
}