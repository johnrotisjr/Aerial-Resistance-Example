using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.State;
using Framework_Module.Interfaces;
using Framework_Module.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ui_Module
{
    /// <summary>
    /// Manages a stack of active screens.
    /// Handles screen registration, push/pop operations, and scene transitions.
    /// </summary>

    public class ScreenManager : MonoBehaviour, IScreenManager
    { 
        private readonly Dictionary<ScreenType, IGameScreen> registeredScreens = new();
        private readonly Stack<IGameScreen> activeScreens = new();
        public IGameScreen TopScreen => activeScreens.Count > 0 ? activeScreens.Peek() : null;

        private EventBus eventBus;
        public Transform Root => transform;
        
        public void Inject(EventBus eventBusService)
        {
            eventBus = eventBusService;
        }
        
        public void Initialize()
        {
            HideAllScreens();
            eventBus.Subscribe<WorldStateChangeEvent>(OnWorldStateChange);
        }

        private void OnWorldStateChange(WorldStateChangeEvent e)
        {
            switch (e.NewState)
            {
                case WorldStateType.Gameover:
                    OpenScreen(ScreenType.MissionComplete);
                    break;
            }
        }

        public void OpenScreen(ScreenType type)
        {
            if (registeredScreens.TryGetValue(type, out var gameScreen))
            {
                if(activeScreens.Count > 0)
                    activeScreens.Peek().HideScreen();
                
                if (activeScreens.Contains(gameScreen))
                {
                    DebugLogger.Log("Screen already active, refreshing...", LogCategory.Ui, LogLevel.Warning);
                    gameScreen.RefreshScreen();
                    return;
                }
                DebugLogger.Log($"Opening Game Screen of type {type}", LogCategory.Ui, LogLevel.Log);
                activeScreens.Push(gameScreen);
                gameScreen.RefreshScreen();
                gameScreen.DisplayScreen();
                return;
            }
            DebugLogger.Log($"Failed to find game screen of type {type}, make sure the game screen is registered first.", LogCategory.Ui, LogLevel.Warning);
        }
        
        public void CloseCurrentScreen()
        {
            if (activeScreens.Count > 0)
            {
                DebugLogger.Log($"Closing Game Screen of type {activeScreens.Peek().GetType()}", LogCategory.Ui, LogLevel.Log);
                var screen = activeScreens.Pop();
                screen.HideScreen();
                if (activeScreens.Count > 0)
                {
                    activeScreens.Peek().RefreshScreen();
                    activeScreens.Peek().DisplayScreen();
                }

                return;
            }
            DebugLogger.Log("Failed to close screen because there are no active screens.", LogCategory.Ui, LogLevel.Warning);
        }

        public void CloseAllScreens()
        {
            while(activeScreens.Count > 0)
            {
                CloseCurrentScreen();
            }
            DebugLogger.Log("Closed all screens.", LogCategory.Ui, LogLevel.Log);
        }
 
        private void HideAllScreens()
        {
            foreach (var registeredScreen in registeredScreens)
            {
                registeredScreen.Value.HideScreen();
            }
            DebugLogger.Log("Hid all screens.", LogCategory.Ui, LogLevel.Log);
        }

        public void RegisterScreen(IGameScreen screen)
        {
            if (registeredScreens.ContainsKey(screen.ScreenType))
            {
                DebugLogger.Log($"Screen of type {screen.GetType()} has already been added!", LogCategory.Ui, LogLevel.Warning);
                return;
            }
            
            if (registeredScreens.TryAdd(screen.ScreenType, screen))
            {
                DebugLogger.Log($"Added Game Screen of type {screen.GetType()}", LogCategory.Ui, LogLevel.Log);
                return;
            }

            DebugLogger.Log($"Failed to add game screen of type {screen.GetType()}.", LogCategory.Ui, LogLevel.Warning);
        }
        
        public void UnregisterScreen(IGameScreen screen)
        {
            if (registeredScreens.Remove(screen.ScreenType))
            {
                DebugLogger.Log($"Removed Game Screen of type {screen.GetType()}", LogCategory.Ui, LogLevel.Log);
                return;
            }
            
            DebugLogger.Log($"Failed to remove game screen of type {screen.GetType()}.", LogCategory.Ui, LogLevel.Log);
        }

        public void Shutdown()
        {
            CloseAllScreens();
            activeScreens.Clear();
            registeredScreens.Clear();
            eventBus.Unsubscribe<WorldStateChangeEvent>(OnWorldStateChange);
        }
    }
}