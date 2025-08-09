using System.Threading.Tasks;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.System;
using Framework_Module.Game_State;
using Framework_Module.Interfaces;
using Framework_Module.Scenes;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

namespace Framework_Module.Core
{
    public class SceneDirector : IGameService
    {
        private SceneTransitionController sceneTransition;
        private SceneLoader sceneLoader;
        private GameStateManager gameStateManager;
        
        public void Initialize()
        {
             
        }

        public void Shutdown()
        {
        }

        public void Inject(SceneLoader sceneLoaderService, SceneTransitionController transitionController, GameStateManager gameStateManagerService)
        {
            gameStateManager = gameStateManagerService;
            sceneTransition = transitionController;
            sceneLoader = sceneLoaderService;
        }
        
        public async Task<bool> Transition(SceneType type, LoadSceneMode mode = LoadSceneMode.Single, bool useLoadScene = false)
        {
            await sceneTransition.FadeOutAsync();
            if (useLoadScene)
            {
                await sceneLoader.LoadSceneAsync(SceneType.Loading);
                await CoroutineRunner.WaitForEndOfFrameAsync(); 
                await sceneTransition.FadeInAsync();
                gameStateManager.ChangeState(GameStateType.Loading);
                var returnVal = await sceneLoader.StartLoadScene(type, mode);
                if (returnVal == null)
                {
                    return false;
                }
                await sceneTransition.FadeOutAsync();
                await sceneLoader.FinishLoadScene(returnVal.Item1, returnVal.Item2);
            }
            else
            {
                await sceneLoader.LoadSceneAsync(type, mode);
            }
            await CoroutineRunner.WaitForEndOfFrameAsync(); 
            await sceneTransition.FadeInAsync();
            return true;
        }
    }
}