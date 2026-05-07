using System;
using Framework_Module.Interfaces;
using UnityEngine;

namespace World_Module.Vehicle_Controller
{
    public abstract class VehicleController : IVehicleController
    {
        public IVehicle ControlledVehicle { get; private set; }
        public abstract void Update();
        public abstract void FixedUpdate();
        
        public virtual void AssignNewVehicle(IVehicle vehicle)
        {
            ControlledVehicle = vehicle;
            
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            ControlledVehicle.DebugSetup(this);
#endif
        }

        public virtual void UnassignVehicle()
        {
            
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            ControlledVehicle?.DebugSetup(null);
#endif
            
            ControlledVehicle = null;
        }
    }
}