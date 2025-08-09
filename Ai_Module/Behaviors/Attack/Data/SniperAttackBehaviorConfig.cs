using Framework_Module.Configs.Ai;
using Framework_Module.Enums;
using UnityEngine;

namespace Ai_Module.Behaviors.Attack.Data
{
    /// <summary>
    /// Configuration for a precise, single-shot attack aimed at the player.
    /// </summary>
    [CreateAssetMenu(fileName = "SniperAttackPatternData", menuName = "Scriptable Objects/Ai/Attack/Patterns/SniperAttackPatternData")]
    public class SniperAttackBehaviorConfig : AiAttackBehaviorConfig
    {
        public override AiAttackType AttackType => AiAttackType.Sniper;
    }
}