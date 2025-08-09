using System.Reflection;
using Debug_Module;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Framework_Module.Service
{
    public abstract class GameServiceBase : MonoBehaviour, IGameService
    {
        private void Awake()
        {
            var method = GetType().GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (method != null)
            {
                DebugLogger.Log($"{GetType().Name}: Is a GameServices and should not implement Awake(). Use Initialize() instead.", LogCategory.Framework, LogLevel.Warning);
            }
        }

        private void OnDestroy()
        {
            var method = GetType().GetMethod("OnDestroy", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (method != null)
            {
                DebugLogger.Log($"{GetType().Name}: Is a GameServices and should not implement OnDestroy(). Use Shutdown() instead.", LogCategory.Framework, LogLevel.Warning);
            }
        }
        
        public abstract void Initialize();
        public abstract void Shutdown();
    }
}