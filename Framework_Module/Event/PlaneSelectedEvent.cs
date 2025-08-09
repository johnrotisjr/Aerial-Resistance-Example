using Framework_Module.Definitions;
using Framework_Module.Interfaces;

namespace Framework_Module.Event
{
    public class PlaneSelectedEvent : IGameEvent
    {
        public VehicleDefinition[] Data;
        public PlaneSelectedEvent(VehicleDefinition[] data)
        {
            Data = data;
        }
    }
}