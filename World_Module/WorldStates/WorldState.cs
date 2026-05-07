using Framework_Module.Enums;
using Framework_Module.Interfaces;

namespace World_Module.WorldStates
{
    public abstract class WorldGameState : IWorldGameState
    {
        protected WorldStateManager WorldStateManager;

        protected WorldGameState(WorldStateManager worldStateManager)
        {
            WorldStateManager = worldStateManager;
        }
        public abstract WorldStateType WorldStateType { get; }
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
        public abstract void FixedUpdate();
    }
}