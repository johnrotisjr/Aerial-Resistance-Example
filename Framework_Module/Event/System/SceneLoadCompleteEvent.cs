using Framework_Module.Enums;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Framework_Module.Event.System
{
    public class SceneLoadCompleteEvent : IGameEvent
    {
        public string LastScene;
        public string NewScene;
        
        public SceneLoadCompleteEvent(string lastScene, string newScene)
        {
            LastScene = lastScene;
            NewScene = newScene;
        }

    }
}