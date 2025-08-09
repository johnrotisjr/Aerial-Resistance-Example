using Framework_Module.Enums;
using UnityEngine.InputSystem;

namespace Framework_Module.Interfaces
{
    public interface IInputController : IGameService
    {
        public void EnablePlayerControls();
        public void DisablePlayerControls();
        public void EnableUIControls();
        public void DisableUIControls();
        public void EnableAllControls();
        public void DisableAllControls();
        public InputAction GetDigitalAction(DigitalInputActionType actionType);
        public InputAction GetAxisAction(AxisInputActionType actionType);
        public string SaveBindingOverridesAsJson();
        public void RemoveAllBindingOverrides();
        public void LoadBindingOverridesFromJson(string json);
    }
}