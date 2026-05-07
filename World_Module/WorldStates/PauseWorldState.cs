using Debug_Module;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.Ui;
using Framework_Module.Interfaces;
using UnityEngine;

namespace World_Module.WorldStates
{
    public class PauseWorldState : WorldGameState
    {
        public override WorldStateType WorldStateType => WorldStateType.Pause;
        private readonly IInputController inputController;
        private readonly EventBus eventBus;
        private readonly IAudio audio;
        
        public PauseWorldState(IInputController inputController, IAudio audio, EventBus eventBus, WorldStateManager worldStateManager) : base(worldStateManager)
        {
            this.inputController = inputController;
            this.eventBus = eventBus;
            this.audio = audio;
        }
        
        public override void Enter()
        {
            DebugLogger.Log("Pause started", LogCategory.Framework, LogLevel.Log);
            Time.timeScale = 0;
            inputController.DisablePlayerControls();
            eventBus.Subscribe<HideScreenEvent>(OnHideScreen);
            audio.MusicPlayer.Pause();
        }

        public override void Exit()
        {
            Time.timeScale = 1;
            inputController.EnablePlayerControls();
            eventBus.Unsubscribe<HideScreenEvent>(OnHideScreen);
            audio.MusicPlayer.Resume();
        }

        private void OnHideScreen(HideScreenEvent e)
        {
            if (e.GameScreen.ScreenType == ScreenType.Pause)
                WorldStateManager.ChangeState(WorldStateType.Play);
        }

        public override void Update()
        {
            
        }

        public override void FixedUpdate()
        {
 
        }
    }
}