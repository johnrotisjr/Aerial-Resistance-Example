using System;
using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Enums;
using Framework_Module.Interfaces;
using UnityEngine;
using UnityEngine.Pool;

namespace Framework_Module.Core
{
    /// <summary>
    /// Central manager for pooling GameObjects. Uses UnityEngine.Pool.ObjectPool internally.
    /// Supports pooling by prefab with optional initialization and reset actions.
    /// </summary>
    public class GameObjectPooler : MonoBehaviour, IGameService
    {
        private readonly Dictionary<PrefabKey, Pool> pool = new();
        public bool IsRegistered(PrefabKey key) => pool.ContainsKey(key);
        public int RegisteredCount => pool.Count;
        
        public void Register(PrefabKey key, GameObject prefab, int defaultCapacity = 10, int maxSize = 100, 
            Action<GameObject> onGet = null, Action<GameObject> onRelease = null, 
            Action<GameObject> onCreate = null)
        {
            if (prefab == null) 
                throw new ArgumentNullException(nameof(prefab));
            
            if (pool.ContainsKey(key))
                return;

            var goContainer = new GameObject($"{prefab.name}_Pool");
            var container = goContainer.transform;
             container.SetParent(transform);

            var objectPool = new ObjectPool<GameObject>(
                createFunc: () =>
                {
                    var instance = Instantiate(prefab, container);
                    instance.SetActive(false);
                    onCreate?.Invoke(instance);
                    return instance;
                },
                actionOnGet: go =>
                {
                    go.SetActive(true);
                    onGet?.Invoke(go);
                },
                actionOnRelease: go =>
                {
                    onRelease?.Invoke(go);
                    go.SetActive(false);
                },
                actionOnDestroy: Destroy,
                collectionCheck: false,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
            );

            this.pool[key] = new Pool { ObjectPool = objectPool, Parent = container };
        }
        
        public bool UnRegister(PrefabKey key)
        {
            if (pool.ContainsKey(key))
            {
                pool[key].ObjectPool.Clear();
                Destroy(pool[key].Parent);
            }
            
            return pool.Remove(key);
        }
    
        public T Get<T>(PrefabKey key) where T : Component
        {
            if (pool.TryGetValue(key, out var p))
            {
                return p.ObjectPool.Get().GetComponent<T>();
            }
            DebugLogger.Log($"Prefab {typeof(T)} not registered with the pooler.", LogCategory.Framework, LogLevel.Log);
            return default;
        }
    
        public void Release(PrefabKey key, GameObject instance)
        {
            if (pool.TryGetValue(key, out var p))
            {
                p.ObjectPool.Release(instance);
            }
            else
            {
                DebugLogger.Log($"Trying to release unregistered prefab: {key.ToString()}", LogCategory.Framework, LogLevel.Log);
                Destroy(instance);
            }
        }

        public void Initialize()
        {
            
        }

        public void Shutdown()
        {
            foreach (var p in pool.Values)
            {
                p.ObjectPool.Clear();
            }
            pool.Clear();
        }
    }
}
