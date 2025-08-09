using Framework_Module.Configs.Ai;
using Framework_Module.Enums;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Data
{
    /// <summary>
    /// Configuration for movement that tracks the player's vertical
    /// position while maintaining horizontal distance.
    /// </summary>
    [CreateAssetMenu(fileName = "StalkerMovementPatternData", menuName = "Scriptable Objects/Ai/Movement/Patterns/StalkerMovementPatternData")]
    public class StalkerMovementBehaviorConfig : AiMovementBehaviorConfig
    {
        public override AiMovementType MovementType  => AiMovementType.Stalker;
    }
}