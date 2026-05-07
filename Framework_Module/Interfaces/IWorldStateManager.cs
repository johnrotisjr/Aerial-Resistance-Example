using Framework_Module.Enums;

namespace Framework_Module.Interfaces
{
    public interface IWorldStateManager : IGameService
    {
        public IWorldGameState CurrentState { get; }
        public void Update();
        public void FixedUpdate();
        public void ClearPausedStates();
        public void ChangeState(WorldStateType newStateType, bool pauseCurrentState = false);
    }
}