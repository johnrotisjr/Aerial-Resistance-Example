
using Framework_Module.Definitions.Behaviors.Movement;

namespace Framework_Module.Interfaces
{
    public interface IMovementAiBehaviorFactory : IBehaviorFactory<IVehicle>
    {
        public IMovementBehavior GetBehavior(MovementAiBehaviorDefinition def);
    }
}