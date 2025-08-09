using Framework_Module.Interfaces;

namespace Framework_Module.Event
{
    public class PickupCollectedEvent : IGameEvent
    {
        public IPickup Pickup;
        public PickupCollectedEvent(IPickup pickup)
        {
            Pickup = pickup;
        }
    }
}