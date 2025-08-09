using Framework_Module.Configs.Ai;
using Framework_Module.Enums;
using UnityEngine;

namespace Ai_Module.Behaviors.Attack.Data
{
    /// <summary>
    /// Configuration data for firing a trio of weapons in a spread
    /// formation.
    /// </summary>
    [CreateAssetMenu(fileName = "SprayAttackPatternData", menuName = "Scriptable Objects/Ai/Attack/Patterns/SprayAttackPatternData")]
    public class SprayAttackBehaviorConfig : AiAttackBehaviorConfig
    {
        public override AiAttackType AttackType => AiAttackType.Spray;
    }
}