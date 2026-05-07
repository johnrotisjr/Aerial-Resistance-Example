using Framework_Module.Interfaces;

namespace Framework_Module.Event.Combat
{
    /// <summary>
    /// Event triggered when any World Object is destroyed.
    /// Used to track mission objectives or clean up references.
    /// </summary>

    //TODO: Cache Events
    public class WorldObjectKilledEvent : IGameEvent
    {
        public readonly IWorldObject WorldObject;
        public WorldObjectKilledEvent(IWorldObject worldObject)
        {
            WorldObject = worldObject;
        }
    }
}