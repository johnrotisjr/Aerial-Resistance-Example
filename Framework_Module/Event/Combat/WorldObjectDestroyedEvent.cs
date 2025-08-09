using Framework_Module.Interfaces;

namespace Framework_Module.Event.Combat
{
    /// <summary>
    /// Event triggered when any entity is destroyed.
    /// Used to track mission objectives or clean up references.
    /// </summary>

    //TODO: Cache Events
    public class WorldObjectDestroyedEvent : IGameEvent
    {
        public readonly IWorldObject WorldObject;
        public WorldObjectDestroyedEvent(IWorldObject worldObject)
        {
            WorldObject = worldObject;
        }
    }
}