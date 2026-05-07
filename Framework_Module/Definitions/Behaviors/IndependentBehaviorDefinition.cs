using System;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Definitions.Behaviors.Movement;
using UnityEngine;

namespace Framework_Module.Definitions
{
    [Serializable]
    public class IndependentBehaviorDefinition
    {
        [SerializeReference] private AiTransitionConditionDefinition transitionConditionDefinition;
        [SerializeField] private float enterTimeDelay;
        [SerializeField] private float exitTimeDelay;
        
        public float EnterTimeDelay => enterTimeDelay;
        public float ExitTimeDelay => exitTimeDelay;
        public AiTransitionConditionDefinition TransitionConditionDefinition => transitionConditionDefinition;
    }
}