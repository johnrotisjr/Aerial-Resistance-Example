using System;
using Framework_Module.Enums;

namespace Framework_Module.Definitions.Behaviors.Attack
{
    [Serializable]
    public sealed class ThrustAttackAiBehaviorDefinition : AttackAiBehaviorDefinition
    {
        public override AiAttackType AttackType => AiAttackType.Thrust;
    }
}