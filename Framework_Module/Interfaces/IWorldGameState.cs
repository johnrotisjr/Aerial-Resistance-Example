using Framework_Module.Enums;

namespace Framework_Module.Interfaces
{
    public interface IWorldGameState : IGameState
    {
        public WorldStateType WorldStateType { get; }
        public void Update();
        public void FixedUpdate();
    }
}