using System;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Definitions.Behaviors.Movement;
using UnityEngine;

namespace Framework_Module.Definitions
{
    [Serializable]
    public class IndependentAttackBehaviorDefinition : IndependentBehaviorDefinition
    {
        [SerializeReference] private AttackAiBehaviorDefinition attackBehaviorDefinition;
        public AttackAiBehaviorDefinition AttackBehaviorDefinition => attackBehaviorDefinition;
    }
}