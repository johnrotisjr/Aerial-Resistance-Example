using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Interfaces;

namespace Framework_Module.Service
{
    /// <summary>
    /// Tracks services that require controlled initialization and shutdown during the application lifecycle.
    /// Internal service management utility.
    /// </summary>

    internal class ServiceLifecycleManager
    {
        private bool bootInitializationComplete = false;
        internal ServiceLifecycleManager() { }
        private readonly HashSet<IGameService> services = new();

        public void TrackService<T>(IGameService service) where T : class, IGameService
        {
            if (bootInitializationComplete)
            {
                service.Initialize();
            }

            services.Add(service as T);
        }
        
        public void UntrackService<T>() where T : class, IGameService
        {
            IGameService service = null;
            foreach (var s in services)
            {
                if (s is not T) 
                    continue;
                
                service = s;
                break;
            }

            if (service == null)
            {
                DebugLogger.Log($"Unable to untrack service of type {typeof(T)}.", LogCategory.Framework, LogLevel.Warning);
                return;
            }
            
            service.Shutdown();
            services.Remove(service);
        }

        public void InitializeServices()
        {
            foreach (var service in services)
            {
                service.Initialize();
            }
            bootInitializationComplete = true;
        }

        public void Shutdown()
        {
            foreach (var service in services)
            {
                service.Shutdown();
            }

            services.Clear();
        }
    }
}
