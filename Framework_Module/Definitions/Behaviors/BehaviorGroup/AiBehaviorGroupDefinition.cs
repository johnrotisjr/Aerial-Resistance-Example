using System;
using Framework_Module.Definitions.Behaviors;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Definitions.Behaviors.Movement;
using UnityEngine;

namespace Framework_Module.Definitions.BehaviorGroup
{
    [Serializable]
    public class AiBehaviorGroupDefinition
    {
        [SerializeReference] private AttackAiBehaviorDefinition attackBehaviorDefinition;
        [SerializeReference] private MovementAiBehaviorDefinition movementBehaviorDefinition;
        [SerializeReference] private AiTransitionConditionDefinition transitionConditionDefinition;
        public AiTransitionConditionDefinition TransitionConditionDefinition => transitionConditionDefinition;
        public AttackAiBehaviorDefinition AttackBehaviorDefinition => attackBehaviorDefinition;
        public MovementAiBehaviorDefinition MovementBehaviorDefinition => movementBehaviorDefinition;
    }
}