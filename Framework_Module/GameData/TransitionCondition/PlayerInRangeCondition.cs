using Framework_Module.Definitions;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.GameData.TransitionCondition
{
    public sealed class PlayerInRangeCondition : AiTransitionCondition
    {
        private PlayerInRangeConditionDefinition def;
        public override AiTransitionType TransitionType => AiTransitionType.PlayerInRange;
        
        public PlayerInRangeCondition(PlayerInRangeConditionDefinition def)
        {
            this.def = def;
        }

        public override bool ConditionMet
        {
            get
            {
                var distance = Vector2.Distance(AiVehicleController.ControlledVehicle.Position, PlayerController.ControlledVehicle.Position);
                bool isInRange = distance < def.DistanceFromPlayer;
                if (def.FlipLogic)
                    return !isInRange;
                
                return isInRange;
            }
        }

 
    }
}