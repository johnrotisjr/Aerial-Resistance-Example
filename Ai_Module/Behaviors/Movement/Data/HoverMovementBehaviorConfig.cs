using Framework_Module.Configs.Ai;
using Framework_Module.Enums;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Data
{
    /// <summary>
    /// Configuration for a hovering movement that oscillates vertically
    /// between two points.
    /// </summary>
    [CreateAssetMenu(fileName = "HoverMovementPatternData", menuName = "Scriptable Objects/Ai/Movement/Patterns/HoverMovementPatternData")]
    public class HoverMovementBehaviorConfig : AiMovementBehaviorConfig
    {
        public override AiMovementType MovementType  => AiMovementType.Hover;
    }
}