using Ai_Module.Behaviors.Movement.Behavior;
using Framework_Module.Configs.Ai;
using Framework_Module.Enums;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Data
{
    /// <summary>
    /// Parameters controlling the amplitude and rate of the vertical
    /// wobble used in <see cref="WobbleMovementBehavior"/>.
    /// </summary>
    [CreateAssetMenu(fileName = "WobbleMovementPatternData", menuName = "Scriptable Objects/Ai/Movement/Patterns/WobbleMovementPatternData")]
    public class WobbleMovementBehaviorConfig : AiMovementBehaviorConfig
    {
        public override AiMovementType MovementType  => AiMovementType.Wobble;
        public float amplitude = 1;
        public float rotationRate = 1;
    }
}