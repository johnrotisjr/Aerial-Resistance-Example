using System;
using Framework_Module.GameData.Ai;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework_Module.Definitions
{
    /// <summary>
    /// Serializable data structure used by AiPhase to define phase-specific behavior in ScriptableObjects.
    /// </summary>

    [Serializable]
    public struct AiPhaseDefinition
    {
        [SerializeField] private AiPhaseConfig aiPhaseConfig;
        [SerializeReference] private AiTransitionConditionDefinition transitionConditionDefinition;

        public AiPhaseConfig AiPhaseConfig => aiPhaseConfig;
        public AiTransitionConditionDefinition TransitionConditionDefinition => transitionConditionDefinition;
        
        public AiPhaseDefinition(AiPhaseConfig aiPhaseConfig, AiTransitionConditionDefinition transitionConditionDefinition)
        {
            this.aiPhaseConfig = aiPhaseConfig;
            this.transitionConditionDefinition = transitionConditionDefinition;
        }
    }
}