using Framework_Module.Interfaces;

namespace Framework_Module.Definitions.BehaviorGroup
{
    public class AiBehaviorGroup
    {
        private IAttackBehavior attackBehavior;
        private IMovementBehavior movementBehavior;
        public IAttackBehavior AttackBehavior => attackBehavior;
        public IMovementBehavior MovementBehavior => movementBehavior;
        private readonly IAttackAiBehaviorFactory attackAiBehaviorFactory;
        private readonly IMovementAiBehaviorFactory movementAiBehaviorFactory;
        
        public AiBehaviorGroup(IAttackAiBehaviorFactory attackFactory, IMovementAiBehaviorFactory movementFactory)
        {
            attackAiBehaviorFactory = attackFactory;
            movementAiBehaviorFactory = movementFactory;
        }

        public void SetBehaviors(AiBehaviorGroupDefinition def)
        {
            movementBehavior = def?.MovementBehaviorDefinition == null ? null : movementAiBehaviorFactory.GetBehavior(def.MovementBehaviorDefinition);
            attackBehavior = def?.AttackBehaviorDefinition == null ? null: attackAiBehaviorFactory.GetBehavior(def.AttackBehaviorDefinition);
        }
    }
}