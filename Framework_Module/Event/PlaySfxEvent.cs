using Framework_Module.Enums;
using Framework_Module.Interfaces;

namespace Framework_Module.Event
{
    public class PlaySfxEvent : IGameEvent
    {
        public readonly AudioSfxType Type;
        public readonly float Volume;
        
        public PlaySfxEvent(AudioSfxType type, float volume = 1.0f)
        {
            Type = type;
            Volume = volume;
        }
    }
}