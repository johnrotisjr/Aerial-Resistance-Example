using Framework_Module.Configs.Ai;
using Framework_Module.Enums;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Data
{
    /// <summary>
    /// Configuration for the kamikaze charge toward the player.
    /// </summary>
    [CreateAssetMenu(fileName = "KamikazeMovementPatternData", menuName = "Scriptable Objects/Ai/Movement/Patterns/KamikazeMovementPatternData")]
    public class KamikazeMovementBehaviorConfig : AiMovementBehaviorConfig
    {
        public override AiMovementType MovementType  => AiMovementType.Kamikaze;
    }
}