using Framework_Module.Definitions;
using Framework_Module.Definitions.Behaviors;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Definitions.Behaviors.Movement;
using Framework_Module.Interfaces;

namespace Framework_Module.GameData.Ai
{
 
    public abstract class VehicleAiBehavior<TDef> : IAiBehavior<IVehicle>, IRebindableAiBehavior<TDef> where TDef : AiBehaviorDefinition
    {
        public abstract void Rebind(TDef aiBehaviorDefinition);
        public abstract void Tick(float deltaTime, IVehicle vehicle);
        public abstract void End(IVehicle vehicle);
        public abstract void Start(IVehicle vehicle);
    }
}