using Framework_Module.Definitions.Behaviors.Attack;

namespace Framework_Module.Interfaces
{
    public interface IAttackAiBehaviorFactory : IBehaviorFactory<IVehicle>
    {
        public IAttackBehavior GetBehavior(AttackAiBehaviorDefinition def);
    }
}