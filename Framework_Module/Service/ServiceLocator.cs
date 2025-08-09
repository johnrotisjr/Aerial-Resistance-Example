using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Debug_Module;
using Framework_Module.Interfaces;

[assembly: InternalsVisibleTo("Game.Tests")]
namespace Framework_Module.Service
{
    /// <summary>
    /// Dependency lookup system allowing systems to retrieve services without direct references.
    /// Part of the service architecture.
    /// </summary>

    internal class ServiceLocator
    {
        private readonly Dictionary<Type, IGameService> services = new();

        /// <summary>
        /// Locates a service
        /// </summary>
        /// <typeparam name="T">Type of service requested</typeparam>
        /// <returns>service of type T or null</returns>
        public T Get<T>() where T : class
        {
            if (services.TryGetValue(typeof(T), out var service))
            {
                return service as T;
            }
            DebugLogger.Log($"Could not find a service of type - {typeof(T)}", LogCategory.Framework, LogLevel.Warning);
            return null;
        }

        /// <summary>
        /// Registers a service
        /// </summary>
        /// <param name="service">Service to be added</param>
        /// <typeparam name="T">Type of Service</typeparam>
        public void Register<T>(IGameService service) where T : class, IGameService
        {
            if (services.TryAdd(typeof(T), service))
            {
                DebugLogger.Log($"Registered service of type - {typeof(T)}", LogCategory.Framework, LogLevel.Log);
                return;
            }
            DebugLogger.Log($"Could not add service of type - {typeof(T)}, Check for a duplicate.", LogCategory.Framework, LogLevel.Error);
        }
        
        /// <summary>
        /// Removes a service from register
        /// </summary>
        /// <typeparam name="T">Type of Service</typeparam>
        public void Unregister<T>() where T : class, IGameService
        {
            if (services.Remove(typeof(T)))
            {
                DebugLogger.Log($"Unregistered service of type - {typeof(T)}", LogCategory.Framework, LogLevel.Log);
                return;
            }
            DebugLogger.Log($"Could not remove service of type - {typeof(T)}, Check if it was added.", LogCategory.Framework, LogLevel.Log);
        }

        public void ClearServices()
        {
            services.Clear();
        }
    }
}
