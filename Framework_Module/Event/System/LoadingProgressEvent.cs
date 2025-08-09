using Framework_Module.Enums;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Framework_Module.Event.System
{
    public class LoadingProgressEvent : IGameEvent
    {
        public float Progress;
        
        public LoadingProgressEvent(float progress)
        {
            Progress = progress;
        }

    }
}