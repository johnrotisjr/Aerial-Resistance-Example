using UnityEngine;

namespace Framework_Module.Interfaces
{
    public interface IUiInputHandler
    {
        void OnSubmit();
        void OnCancel();
        void OnPause();
        void OnDebug();
        void OnNavigate(Vector2 direction);
    }
}