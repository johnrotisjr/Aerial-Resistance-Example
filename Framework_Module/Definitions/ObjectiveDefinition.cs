using System;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct ObjectiveDefinition
    {
        [SerializeField] private ObjectiveTaskType taskType;
        [SerializeField] private int targetValue;
        
        [SerializeField] string displayName;
        public string DisplayName => displayName;        
        
        public ObjectiveTaskType TaskType => taskType;
        public int TargetValue => targetValue;

        public ObjectiveDefinition(string displayName, ObjectiveTaskType taskType, int targetValue)
        {
            this.displayName = displayName;
            this.taskType = taskType;
            this.targetValue = targetValue;
            
        }
    }
}