using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Game_State;
using Framework_Module.Interfaces;
using Framework_Module.Scenes;
using Framework_Module.Service;
using UnityEngine;

namespace Input_Module
{
    public class UiInputHandler : IUiInputHandler
    {
        private readonly GameStateManager gameStateManager;
        private readonly IScreenManager screenManager;
        private readonly SceneLoader sceneLoader;
        private bool isDebugActive = false;
        private bool debugTaskActive = false;

        public UiInputHandler(GameStateManager gameStateManager, IScreenManager screenManager, SceneLoader sceneLoader)
        {
            this.gameStateManager = gameStateManager;
            this.screenManager = screenManager;
            this.sceneLoader = sceneLoader;
        }

        public void OnSubmit()
        {
            var state = gameStateManager.CurrentState.GameStateType;
            if (state == GameStateType.Title)
            {
                gameStateManager.ChangeState(GameStateType.Menu);
                return;
            }

            screenManager.TopScreen?.OnSubmit(true);
        }

        public void OnCancel()
        {
            screenManager.CloseCurrentScreen();
        }

        public void OnPause()
        {
            screenManager.OpenScreen(ScreenType.Pause);
        }

        public async void OnDebug()
        {
            if (debugTaskActive) return;
            debugTaskActive = true;

            if (!isDebugActive)
            {
                await sceneLoader.LoadSceneAsync(SceneType.Debug, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                isDebugActive = true;
            }
            else
            {
                await sceneLoader.UnloadSceneAsync(SceneType.Debug);
                isDebugActive = false;
            }

            debugTaskActive = false;
        }

        public void OnNavigate(Vector2 direction)
        {
            screenManager.TopScreen?.OnNavigate(direction);
        }
    }
}