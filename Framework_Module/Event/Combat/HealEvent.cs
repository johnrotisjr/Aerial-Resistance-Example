using Framework_Module.Interfaces;

namespace Framework_Module.Event.Combat
{
    /// <summary>
    /// Event triggered when an entity heals.
    /// Includes reference to the healed Vehicle and amount of healing.
    /// </summary>

    public class HealEvent : IGameEvent
    {
        public readonly IWorldObject WorldObject;
        public readonly float Damage;
        
        public HealEvent(IWorldObject worldObject, float damage)
        {
            WorldObject = worldObject;
            Damage = damage;
        }
    }
}