using Framework_Module.Interfaces;

namespace Framework_Module.Event.State
{
    /// <summary>
    /// Event signaling that the game or mission has ended (victory or defeat).
    /// Triggers transition to end-of-game flows.
    /// </summary>

    //TODO: Cache Events
    public class GameOverEvent : IGameEvent
    {
        public readonly bool MissionComplete;

        public GameOverEvent(bool missionComplete)
        {
            MissionComplete = missionComplete;
        }
    }
}
