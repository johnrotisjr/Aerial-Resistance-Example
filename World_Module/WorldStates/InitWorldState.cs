
using Framework_Module.Enums;
using Framework_Module.Interfaces;

namespace World_Module.WorldStates
{
    public class InitWorldState : WorldGameState
    {
        public override WorldStateType WorldStateType => WorldStateType.Init;
        private readonly IInputController inputController;
        
        public InitWorldState(WorldStateManager worldStateManager) : base(worldStateManager) { }
        
        public override void Enter()
        {

        }

        public override void Exit()
        {
 
        }

        public override void Update()
        {
            WorldStateManager.ChangeState(WorldStateType.Play);
        }

        public override void FixedUpdate()
        {
 
        }
    }
}