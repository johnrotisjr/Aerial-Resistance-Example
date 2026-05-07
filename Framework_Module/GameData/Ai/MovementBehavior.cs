using Framework_Module.Definitions.Behaviors.Movement;
using Framework_Module.Interfaces;

namespace Framework_Module.GameData.Ai
{
    public abstract class MovementBehavior<TDef> : VehicleAiBehavior<TDef>, IMovementBehavior where TDef : MovementAiBehaviorDefinition
    {
        protected TDef Def;
        public event IMovementBehavior.CompletedMovementCycle OnCompletedMovementCycle;
        protected void RaiseCompletedMovementCycle() => OnCompletedMovementCycle?.Invoke();
        public override void Start(IVehicle vehicle)
        {
            vehicle?.ModifyMovementSpeed(Def.MovementSpeedAdjustment, Def.IsAdditiveAdjustment);
        }

        public override void End(IVehicle vehicle)
        {
            vehicle?.ResetMovementSpeed();
        }
    }
}