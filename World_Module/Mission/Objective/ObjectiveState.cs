using Framework_Module.Definitions;
using Framework_Module.Enums;
using Framework_Module.Interfaces;
using UnityEngine;

namespace World_Module.Mission.Objective
{
    /// <summary>
    /// Represents the runtime state of a specific mission objective, tracking current value and completion.
    /// </summary>

    internal class ObjectiveState : IObjectiveState
    {
        public ObjectiveDefinition Objective { get; }
        private float currentValue;
        public bool IsComplete => currentValue >= Objective.TargetValue;
        public float CurrentValue => currentValue;
        public ObjectiveType ObjectiveType { get; }

        public ObjectiveState(ObjectiveDefinition objective, ObjectiveType type)
        {
            Objective = objective;
            ObjectiveType = type;
        }

        public void UpdateObjective(float amount)
        {
            currentValue += amount;
            currentValue = Mathf.Min(currentValue, Objective.TargetValue);
        }
    }
}