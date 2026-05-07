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

        public ObjectiveTaskType TaskType => taskType;
        public int TargetValue => targetValue;

        public ObjectiveDefinition(ObjectiveTaskType taskType, int targetValue)
        {
            this.taskType = taskType;
            this.targetValue = targetValue;
            
        }
    }
}