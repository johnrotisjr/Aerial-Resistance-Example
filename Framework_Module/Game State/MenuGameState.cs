using Framework_Module.Core;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.Ui;
using Framework_Module.Interfaces;

namespace Framework_Module.Game_State
{
    public class MenuGameState : GameState
    {
        public override GameStateType GameStateType => GameStateType.Menu;
        private readonly EventBus eventBus;
        private readonly IInputController inputController;

        public MenuGameState(SceneDirector sceneDirector, GameStateManager gameStateManager, IInputController inputController,
            EventBus eventBus) : base(sceneDirector, gameStateManager, inputController)
        {
            this.eventBus = eventBus;
            this.inputController = inputController;
        }

        private async void OnNewGameSelected(NewGameSelectedEvent e)
        {
            await SceneDirector.Transition(SceneType.MissionHub);
            GameStateManager.ChangeState(GameStateType.MissionHub);
        }
        
        
        public override void Enter()
        {
            inputController.EnableUIControls();
            eventBus.Subscribe<NewGameSelectedEvent>(OnNewGameSelected);
        }


        public override void Exit()
        {
            inputController.DisableUIControls();
            eventBus.Unsubscribe<NewGameSelectedEvent>(OnNewGameSelected);
        }
    }
}