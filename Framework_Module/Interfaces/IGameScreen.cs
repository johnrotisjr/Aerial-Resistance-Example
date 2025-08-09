using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Interfaces
{
    public interface IGameScreen
    {
        ScreenType ScreenType { get; }
        public void DisplayScreen();
        public void HideScreen();
        public void RefreshScreen();
        public void OnSubmit(bool isPressed);
        public void OnNavigate(Vector2 inputValue);
    }
}
