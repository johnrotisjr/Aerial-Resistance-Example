using Framework_Module.Interfaces;

namespace Framework_Module.Event
{
    public class MissionSelectedEvent : IGameEvent
    {
        public int MissionIndex;

        public MissionSelectedEvent(int missionIndex)
        {
            MissionIndex = missionIndex;
        }
    }
}