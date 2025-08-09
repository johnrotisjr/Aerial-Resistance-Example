using Framework_Module.Enums;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Framework_Module.Interfaces
{
    public interface IScreenManager : IGameService
    {
        public Transform Root { get; }
        public IGameScreen TopScreen { get; }
        public void OpenScreen(ScreenType type);
        public void CloseCurrentScreen();
        public void CloseAllScreens();
        public void RegisterScreen(IGameScreen screen);
        public void UnregisterScreen(IGameScreen screen);
    }
}