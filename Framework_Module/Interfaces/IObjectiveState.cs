using Framework_Module.Definitions;
using Framework_Module.Enums;

namespace Framework_Module.Interfaces
{

    public interface IObjectiveState
    {
        public ObjectiveDefinition Objective { get; }
        public bool IsComplete { get; }
        public float CurrentValue { get; }
        public ObjectiveType ObjectiveType { get; }
        public void UpdateObjective(float amount);
    }
}