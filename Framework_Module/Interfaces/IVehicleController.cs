using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Interfaces
{
    public interface IVehicleController : IUpdateable
    {
        public IVehicle ControlledVehicle { get; }
        public void AssignNewVehicle(IVehicle vehicle);
        public void UnassignVehicle();
    }
}