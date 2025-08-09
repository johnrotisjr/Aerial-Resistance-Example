using Framework_Module.Configs.Ai;
using Framework_Module.Enums;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Data
{
    /// <summary>
    /// Settings for movement that guides the vehicle to a specific
    /// coordinate in world space.
    /// </summary>
    [CreateAssetMenu(fileName = "MoveToPositionMovementPatternData", menuName = "Scriptable Objects/Ai/Movement/Patterns/MoveToPositionMovementPatternData")]
    public class MoveToPositionMovementBehaviorConfig : AiMovementBehaviorConfig
    {
        public Vector2 endPosition;        
        public override AiMovementType MovementType  => AiMovementType.MoveToPosition;
    }
}