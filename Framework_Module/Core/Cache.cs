using System;
using System.Collections.Generic;
using Debug_Module;

namespace Framework_Module.Core
{
    public class Cache<TKey, TValue> where TValue : class
    {
        private readonly Dictionary<TKey, TValue> cache = new();

        private readonly Func<TKey, TValue> factory;
 
        public Cache() { }
        
        public Cache(Func<TKey, TValue> factory = null)
        {
            this.factory = factory;
        }

        public TReturnType Get<TReturnType>(TKey key) where TReturnType : TValue, new()
        {
            if (cache.TryGetValue(key, out var instance) && instance is TReturnType r)
            {
                return r;
            }

            var returnType = new TReturnType();
            Add(key, returnType);
            return returnType;
        }
        
        public TValue Get(TKey key) 
        {
            if (factory == null)
            {
                DebugLogger.Log("Factory is required if not specifying the Return type", LogCategory.Framework, LogLevel.Error);
                return null;
            }
            
            if (cache.TryGetValue(key, out var instance))
            {
                return instance;
            }

            var returnType = factory.Invoke(key);
            Add(key, returnType);
            return returnType;
        }

        public Dictionary<TKey, TValue>.ValueCollection GetValues()
        {
            return cache?.Values;
        }
        
        private void Add(TKey key, TValue instance)
        {
            if (!cache.TryAdd(key, instance))
                cache[key] = instance;
        }
        
        private bool Remove(TKey t)
        {
            return cache.Remove(t);
        }
        
        public void Clear()
        {
            cache.Clear();
        }
    }
}