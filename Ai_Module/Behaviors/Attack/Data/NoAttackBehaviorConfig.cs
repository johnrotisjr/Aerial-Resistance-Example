using Framework_Module.Configs.Ai;
using Framework_Module.Enums;
using UnityEngine;

namespace Ai_Module.Behaviors.Attack.Data
{
    /// <summary>
    /// Configuration for an attack behavior that does nothing.
    /// </summary>
    [CreateAssetMenu(fileName = "NoAttackPatternData", menuName = "Scriptable Objects/Ai/Attack/Patterns/NoAttackPatternData")]
    public class NoAttackBehaviorConfig : AiAttackBehaviorConfig
    {
        public override AiAttackType AttackType => AiAttackType.None;
    }
}