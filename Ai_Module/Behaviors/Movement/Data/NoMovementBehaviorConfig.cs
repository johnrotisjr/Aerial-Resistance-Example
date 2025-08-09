using Framework_Module.Configs.Ai;
using Framework_Module.Enums;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Data
{
    /// <summary>
    /// Data for a passive behavior where the vehicle drifts according to a
    /// preset direction without additional logic.
    /// </summary>
    [CreateAssetMenu(fileName = "NoMovementPatternData", menuName = "Scriptable Objects/Ai/Movement/Patterns/NoMovementPatternData")]
    public class NoMovementBehaviorConfig : AiMovementBehaviorConfig
    {
        public override AiMovementType MovementType  => AiMovementType.None;
        public Vector3 normalizedDriftDirection;
    }
}