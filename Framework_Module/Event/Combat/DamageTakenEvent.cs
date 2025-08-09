using Framework_Module.Interfaces;

namespace Framework_Module.Event.Combat
{
    /// <summary>
    /// Event triggered when an entity takes damage.
    /// Includes reference to the damaged Vehicle and amount of damage taken.
    /// </summary>

    public class DamageTakenEvent : IGameEvent
    {
        public readonly IWorldObject WorldObject;
        public readonly float Damage;
        
        public DamageTakenEvent(IWorldObject worldObject, float damage)
        {
            WorldObject = worldObject;
            Damage = damage;
        }
    }
}