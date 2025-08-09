using Framework_Module.Interfaces;

namespace Framework_Module.Event.Combat
{
    /// <summary>
    /// Event triggered when a boss is destroyed.
    /// Used to signal mission progression or completion.
    /// </summary>

    public class BossDestroyedEvent : IGameEvent
    {
        public readonly IWorldObject WorldObject;
        public BossDestroyedEvent(IWorldObject worldObject)
        {
            WorldObject = worldObject;
        }
    }
}