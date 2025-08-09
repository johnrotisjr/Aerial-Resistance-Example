using Framework_Module.Configs.Ai;
using Framework_Module.Enums;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Data
{
    /// <summary>
    /// Configuration for a retreat maneuver that moves the vehicle
    /// away from the action after attacking.
    /// </summary>
    [CreateAssetMenu(fileName = "RetreatMovementPatternData", menuName = "Scriptable Objects/Ai/Movement/Patterns/RetreatMovementPatternData")]
    public class RetreatMovementBehaviorConfig : AiMovementBehaviorConfig
    {
        public override AiMovementType MovementType  => AiMovementType.Retreat;
    }
}