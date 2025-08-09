using System;
using Framework_Module.Configs.Ai;
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
        [FormerlySerializedAs("conditionData")] [SerializeField] private AiTransitionConditionDefinition conditionDefinition;

        public AiPhaseConfig AiPhaseConfig => aiPhaseConfig;
        public AiTransitionConditionDefinition ConditionDefinition => conditionDefinition;
        
        public AiPhaseDefinition(AiPhaseConfig phaseConfig, AiTransitionConditionDefinition conditionDefinition)
        {
            this.aiPhaseConfig = phaseConfig;
            this.conditionDefinition = conditionDefinition;
        }
    }
}