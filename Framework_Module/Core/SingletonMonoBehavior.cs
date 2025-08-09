using UnityEngine;

namespace Framework_Module.Core
{
    public class SingletonMonoBehavior<T> : MonoBehaviour where T : SingletonMonoBehavior<T>
    {
        private static T _instance;
        private static readonly object Lock = new();
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        var obj = new GameObject(typeof(T).Name);
                        _instance = obj.AddComponent<T>();
                        DontDestroyOnLoad(obj);
                    }
                }

                return _instance;
            }
        }
        
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = (T)this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
 
        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
    }
}