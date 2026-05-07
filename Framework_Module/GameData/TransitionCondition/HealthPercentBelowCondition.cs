using Framework_Module.Definitions;
using Framework_Module.Enums;

namespace Framework_Module.GameData.TransitionCondition
{
    public sealed class HealthPercentBelowCondition : AiTransitionCondition
    {
        private HealthPercentBelowConditionDefinition def;
        public HealthPercentBelowCondition(HealthPercentBelowConditionDefinition def)
        {
            this.def = def;
        }
        public override AiTransitionType TransitionType => AiTransitionType.HealthPercentBelow;
        public override bool ConditionMet 
        {
            get
            {
                var aiHealth = AiVehicleController.ControlledVehicle.Health;
                var targetHealth = def.Percent * AiVehicleController.ControlledVehicle.MaxHealth;
                bool isBelowHealth = aiHealth < targetHealth;
                if (def.FlipLogic)
                    return !isBelowHealth;
                return isBelowHealth;
            }
        }
 
    }
}