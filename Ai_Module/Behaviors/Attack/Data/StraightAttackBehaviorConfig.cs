using Framework_Module.Configs.Ai;
using Framework_Module.Enums;
using UnityEngine;

namespace Ai_Module.Behaviors.Attack.Data
{
    /// <summary>
    /// Configuration for a straightforward weapon fired along the
    /// entity's facing direction.
    /// </summary>
    [CreateAssetMenu(fileName = "StraightAttackPatternData", menuName = "Scriptable Objects/Ai/Attack/Patterns/StraightAttackPatternData")]
    public class StraightAttackBehaviorConfig : AiAttackBehaviorConfig
    {
        public override AiAttackType AttackType => AiAttackType.Straight;
    }
}