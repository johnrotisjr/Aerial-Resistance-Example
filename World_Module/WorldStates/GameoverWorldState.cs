 
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.Gameplay;

namespace World_Module.WorldStates
{
    public class GameoverWorldState : WorldGameState
    {
        private readonly EventBus eventBus;
        public override WorldStateType WorldStateType => WorldStateType.Gameover;
        
        public GameoverWorldState(EventBus eventBus, WorldStateManager worldStateManager) : base(worldStateManager)
        {
            this.eventBus = eventBus;
        }
        
        public override void Enter()
        {
            eventBus.Subscribe<RestartMissionEvent>(OnRestartMissionEvent);
        }

        public override void Exit()
        {
            eventBus.Unsubscribe<RestartMissionEvent>(OnRestartMissionEvent);
        }
        
        private void OnRestartMissionEvent(RestartMissionEvent e)
        {
            WorldStateManager.ClearPausedStates();
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