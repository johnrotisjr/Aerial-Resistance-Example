using System;
using Framework_Module.GameData.Ai;
using UnityEngine;

namespace Framework_Module.Definitions
{
    /// <summary>
    /// Serializable data structure used by AiRoutine to define routine-specific behavior in ScriptableObjects.
    /// </summary>

    [Serializable]
    public class AiRoutineDefinition
    {
        [SerializeField] private AiRoutineConfig aiRoutineConfig;
        [SerializeReference] private AiTransitionConditionDefinition transitionConditionDefinition;
        
        public AiRoutineConfig AiRoutineConfig => aiRoutineConfig;
        public AiTransitionConditionDefinition TransitionConditionDefinition => transitionConditionDefinition;
        public AiRoutineDefinition(AiRoutineConfig aiRoutineConfig, AiTransitionConditionDefinition transitionConditionDefinition)
        {
            this.aiRoutineConfig = aiRoutineConfig;
            this.transitionConditionDefinition = transitionConditionDefinition;
        }
    }
}