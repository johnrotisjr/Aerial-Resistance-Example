using Framework_Module.Game_State;

namespace Framework_Module.Interfaces
{
    public interface IWorldGameState : IGameState
    {
        public void Tick();
        public void FixedTick();
    }
}