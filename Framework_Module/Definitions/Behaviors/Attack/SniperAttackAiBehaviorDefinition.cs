using System;
using Framework_Module.Enums;

namespace Framework_Module.Definitions.Behaviors.Attack
{
    [Serializable]
    public sealed class SniperAttackAiBehaviorDefinition : AttackAiBehaviorDefinition
    {
        public override AiAttackType AttackType => AiAttackType.Sniper;
    }
    
}