using System;
using Framework_Module.Configs.Ai;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework_Module.Definitions
{
    /// <summary>
    /// Serializable data structure used by AiRoutine to define routine-specific behavior in ScriptableObjects.
    /// </summary>

    [Serializable]
    public struct AiRoutineDefinition
    {
        [FormerlySerializedAs("AIRoutineConfig")] [SerializeField] private AiRoutineConfig aiRoutineConfig;
        [FormerlySerializedAs("ConditionDefinition")] [SerializeField] private AiTransitionConditionDefinition conditionDefinition;
        
        public AiRoutineConfig AiRoutineConfig => aiRoutineConfig;
        public AiTransitionConditionDefinition ConditionDefinition => conditionDefinition;
        
        public AiRoutineDefinition(AiRoutineConfig routineConfig, AiTransitionConditionDefinition conditionDefinition)
        {
            this.aiRoutineConfig = routineConfig;
            this.conditionDefinition = conditionDefinition;
        }
    }
}