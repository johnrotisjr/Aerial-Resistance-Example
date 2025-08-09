using Framework_Module.Configs.Ai;
using Framework_Module.Interfaces;

namespace Ai_Module
{
    /// <summary>
    /// Executes the current AI pattern by invoking associated movement and attack behaviors.
    /// Bridges AI logic flow to actual behavior execution.
    /// </summary>

    public class AiBehaviorExecutor
    {
        private readonly IVehicle vehicle;
        private IMovementBehavior movementBehavior;
        private IAttackBehavior attackBehavior;
        private readonly IAttackBehaviorFactory attackBehaviorFactory;
        private readonly IMovementBehaviorFactory movementBehaviorFactory;

        public int MovementCycles => movementBehavior?.CompletedCycles ?? 0;
        
        public AiBehaviorExecutor(IVehicle vehicle, IAttackBehaviorFactory attackBehaviorFactory, IMovementBehaviorFactory movementBehaviorFactory)
        {
            this.vehicle = vehicle;
            this.attackBehaviorFactory = attackBehaviorFactory;
            this.movementBehaviorFactory = movementBehaviorFactory;
        }

        public void Apply(AiPatternPairConfig patternConfig)
        {
            movementBehavior?.EndBehavior(vehicle);
            attackBehavior?.EndBehavior(vehicle);
            
            movementBehavior = movementBehaviorFactory.GetBehavior(patternConfig.aiMovementBehaviorConfig);
            attackBehavior = attackBehaviorFactory.GetBehavior(patternConfig.aiAttackBehaviorConfig);
        }

        public void Update()
        {
            movementBehavior?.Execute(vehicle);
            attackBehavior?.Execute(vehicle);
        }
    }
}