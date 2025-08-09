using Framework_Module.Configs.Ai;
using Framework_Module.Enums;
using UnityEngine;

namespace Ai_Module.Behaviors.Attack.Data
{
    /// <summary>
    /// Defines settings for an attack that fires weapons outward in
    /// multiple directions simultaneously.
    /// </summary>
    [CreateAssetMenu(fileName = "RadialAttackPatternData", menuName = "Scriptable Objects/Ai/Attack/Patterns/RadialAttackPatternData")]
    public class RadialAttackBehaviorConfig : AiAttackBehaviorConfig
    {
        public override AiAttackType AttackType => AiAttackType.Radial;
    }
}