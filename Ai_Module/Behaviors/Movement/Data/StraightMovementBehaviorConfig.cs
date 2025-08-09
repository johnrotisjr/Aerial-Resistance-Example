using Framework_Module.Configs.Ai;
using Framework_Module.Enums;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Data
{
    /// <summary>
    /// Configuration for basic straight-line movement in the
    /// facing direction of the vehicle. 
    /// </summary>
    [CreateAssetMenu(fileName = "StraightMovementPatternData", menuName = "Scriptable Objects/Ai/Movement/Patterns/StraightMovementPatternData")]
    public class StraightMovementBehaviorConfig : AiMovementBehaviorConfig
    {
        public override AiMovementType MovementType  => AiMovementType.Straight;
    }
}