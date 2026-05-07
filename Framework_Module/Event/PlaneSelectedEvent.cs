using Framework_Module.GameData.Data;
using Framework_Module.Interfaces;

namespace Framework_Module.Event
{
    public class PlaneSelectedEvent : IGameEvent
    {
        public VehicleData Data;
        public PlaneSelectedEvent(VehicleData data)
        {
            Data = data;
        }
    }
}