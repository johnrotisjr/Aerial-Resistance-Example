using System;
using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Core;
using Framework_Module.Interfaces;

namespace Framework_Module.Event
{
    /// <summary>
    /// A lightweight publish-subscribe event system used for decoupled communication between systems.
    /// Supports both persistent and one-time event listeners.
    /// </summary>
    
    public class EventBus : IGameService
    {
        private readonly Cache<Type, List<Subscription>> subscribers = new((t)=>new List<Subscription>());
        
        public void Initialize() { }

        public void Shutdown()
        {
            subscribers.Clear();
        }

        public void SubscribeOnce<T>(Action<T> callback) where T : IGameEvent
        {
            subscribers.Get(typeof(T)).Add(new Subscription(callback, true));
        }

        public void Subscribe<T>(Action<T> callback) where T : IGameEvent
        {
            subscribers.Get(typeof(T)).Add(new Subscription(callback, false));
        }

        public void Unsubscribe<T>(Action<T> callback) where T : IGameEvent
        {
            var list = subscribers.Get(typeof(T));
            list.RemoveAll(s => s.Delegate == (Delegate)callback);
        }

        public void UnsubscribeAll()
        {
            foreach (var subscriberList in subscribers.GetValues())
            {
                subscriberList.Clear();
            }
        }

        public virtual void Publish<T>(T gameEvent) where T : IGameEvent
        {
            DebugLogger.Log($"{typeof(T)} Published", LogCategory.Framework, LogLevel.Log);

            var list = subscribers.Get(typeof(T));
            
            for (var i = 0; list.Count > 0 && i < list.Count; i++)
            {
                if (list[i].Delegate is not Action<T> action) 
                    continue;
                
                action.Invoke(gameEvent);

                if (i < list.Count && list[i].IsOneTimeOnly)
                {
                    list.RemoveAt(i);
                    i--;   
                }
            }
        }

    }
}
