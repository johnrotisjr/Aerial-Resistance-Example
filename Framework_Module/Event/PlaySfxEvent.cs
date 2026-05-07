using Framework_Module.Enums;
using Framework_Module.Interfaces;

namespace Framework_Module.Event
{
    public class PlaySfxEvent : IGameEvent
    {
        public readonly AudioSfxType Type;

        public PlaySfxEvent(AudioSfxType type)
        {
            Type = type;
        }
    }
}