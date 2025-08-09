using Framework_Module.Enums;
using Framework_Module.Interfaces;

namespace Framework_Module.Event.Objective
{
    /// <summary>
    /// Event triggered when a mission objective is completed.
    /// Used by the HUD and mission logic systems.
    /// </summary>

    //TODO: Cache Events
    public class ObjectiveCompleteEvent : IGameEvent
    {
        public readonly IObjectiveState State;
        public ObjectiveCompleteEvent(IObjectiveState objectiveState)
        {
            State = objectiveState;
        }
    }
}
