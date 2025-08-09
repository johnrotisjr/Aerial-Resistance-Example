using Framework_Module.Interfaces;
using UnityEngine;

namespace Framework_Module.Event.System
{
    public class ResolutionChangedEvent : IGameEvent
    {
        public Vector2Int Resolution;

        public ResolutionChangedEvent(Vector2Int resolution)
        {
            Resolution = resolution;
        }

    }
}