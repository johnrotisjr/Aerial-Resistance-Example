using System;
using System.Threading.Tasks;
using Debug_Module;
using Framework_Module.Enums;
using Framework_Module.Interfaces;
using UnityEngine.SceneManagement;
using Framework_Module.Core;
using Framework_Module.Event;
using Framework_Module.Event.System;
using Framework_Module.Extensions;
using UnityEngine;

namespace Framework_Module.Scenes
{
    public class SceneLoader : IGameService
    {
        private readonly EventBus eventBus;
        
        public SceneLoader(EventBus eventBus)
        {
            this.eventBus = eventBus;
        }
        
        public void Initialize()
        { 
        }

        public void Shutdown()
        {
            
        }
        
        public async Task FinishLoadScene(AsyncOperation asyncOperation, string oldSceneName)
        {
            asyncOperation.allowSceneActivation = true;
            
            while (!asyncOperation.isDone)
            {
                await Task.Yield(); 
            }
            
            string newSceneName = SceneManager.GetActiveScene().name;
            eventBus.Publish(new SceneLoadCompleteEvent(oldSceneName, newSceneName));
        }

        public async Task<Tuple<AsyncOperation, string>> StartLoadScene(SceneType type, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (SceneManager.GetActiveScene().name == type.SceneTypeToNamed())
                return null;
            
            string oldSceneName = SceneManager.GetActiveScene().name;
            var loadOp = SceneManager.LoadSceneAsync(type.SceneTypeToNamed(), mode);
            if (loadOp == null)
            {
                DebugLogger.Log("System error while trying to load scene", LogCategory.Framework, LogLevel.Error);
                return null;
            }
            loadOp.allowSceneActivation = false;
            while (loadOp.progress < 0.9f)
            {
                float progress = Mathf.Clamp01(loadOp.progress / 0.9f);
                eventBus.Publish(new LoadingProgressEvent(progress));
                await Task.Yield();
            }
            
            eventBus.Publish(new LoadingProgressEvent(1));
            return Tuple.Create(loadOp, oldSceneName);
        }
        
        public async Task LoadSceneAsync(SceneType type, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (SceneManager.GetActiveScene().name == type.SceneTypeToNamed())
                return;
            
            var returnVal = await StartLoadScene(type, mode);
            await FinishLoadScene(returnVal.Item1, returnVal.Item2);
        }
        
        public async Task UnloadSceneAsync(SceneType type)
        {
            var scene = SceneManager.GetSceneByName(type.SceneTypeToNamed());
            await SceneManager.UnloadSceneAsync(scene);
        }
    }
}