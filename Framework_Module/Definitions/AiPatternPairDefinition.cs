using System;
using Framework_Module.Configs.Ai;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework_Module.Definitions
{
    /// <summary>
    /// Serializable data structure for storing pattern metadata used by AiPattern.
    /// </summary>

    [Serializable]
    public struct AiPatternPairDefinition
    {
        [SerializeField] private AiPatternPairConfig aiPatternPairConfig;
        [FormerlySerializedAs("conditionData")] [SerializeField] private AiTransitionConditionDefinition conditionDefinition;

        public AiPatternPairConfig AiPatternPairConfig => aiPatternPairConfig;
        public AiTransitionConditionDefinition ConditionDefinition => conditionDefinition;
        
        public AiPatternPairDefinition(AiPatternPairConfig pairConfig, AiTransitionConditionDefinition conditionDefinition)
        {
            this.aiPatternPairConfig = pairConfig;
            this.conditionDefinition = conditionDefinition;
        }
    }
}