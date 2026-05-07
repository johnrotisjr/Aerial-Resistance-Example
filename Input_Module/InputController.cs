using Debug_Module;
using Framework_Module.Enums;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input_Module
{
    /// <summary>
    /// Unity MonoBehaviour that initializes and manages input action callbacks for gameplay and UI.
    /// Routes raw input to the InputDispatcher and controls input scheme toggling.
    /// </summary>

    public class InputController : GameServiceBase, IInputController
    {
        private PlayerInputControls inputControls;
        private InputDispatcher dispatcher;

        public void EnablePlayerControls() => inputControls?.Player.Enable();
        public void DisablePlayerControls() => inputControls?.Player.Disable();
        public void EnableUIControls() => inputControls?.UI.Enable();
        public void DisableUIControls() => inputControls?.UI.Disable();

        public void Inject(InputDispatcher inputDispatcher)
        {
            dispatcher = inputDispatcher;
        }
        
        public void EnableAllControls()
        {
            EnableUIControls();
            EnablePlayerControls();
        }

        public void DisableAllControls()
        {
            DisablePlayerControls();
            DisableUIControls();
        }
        
        public InputAction GetDigitalAction(DigitalInputActionType actionType)
        {
            return actionType switch
            {
                DigitalInputActionType.FireGun => inputControls.Player.FireGun,
                DigitalInputActionType.FireWeapon => inputControls.Player.FireWeapon,
                DigitalInputActionType.Pause => inputControls.Player.Pause,
                DigitalInputActionType.Submit => inputControls.UI.Submit,
                DigitalInputActionType.Cancel => inputControls.UI.Cancel,
                DigitalInputActionType.Debug => inputControls.UI.Debug,
                _ => null
            };
        }
        
        public InputAction GetAxisAction(AxisInputActionType actionType)
        {
            return actionType switch
            {
                AxisInputActionType.Move => inputControls.Player.Move,
                AxisInputActionType.Navigate => inputControls.UI.Navigate,
                _ => null
            };
        }

        public override void Initialize()
        {
            inputControls = new PlayerInputControls();
        }

        private void Start()
        {
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            inputControls.Player.FireGun.performed += OnFireStart;
            inputControls.Player.FireGun.canceled += OnFireEnd;
            inputControls.Player.FireWeapon.performed += OnFireWeapon;
            inputControls.Player.ChangeWeapon.performed += OnChangeWeapon;
            inputControls.Player.Move.canceled += OnMove;
            inputControls.Player.Move.performed += OnMove;
            inputControls.Player.Pause.performed += OnPause;
            inputControls.UI.Submit.performed += OnSubmit;
            inputControls.UI.Cancel.performed += OnCancel;
            inputControls.UI.Navigate.performed += OnNavigate;
            inputControls.UI.Debug.performed += OnDebug;
        }
        
        private void UnRegisterEvents()
        {
            inputControls.Player.Move.performed -= OnMove;
            inputControls.Player.Move.canceled -= OnMove;
            inputControls.Player.Pause.performed -= OnPause;
            inputControls.Player.FireGun.performed -= OnFireStart;
            inputControls.Player.FireGun.canceled -= OnFireEnd;
            inputControls.Player.FireWeapon.performed -= OnFireWeapon;
            inputControls.Player.ChangeWeapon.performed -= OnChangeWeapon;
            inputControls.UI.Submit.performed -= OnSubmit;
            inputControls.UI.Cancel.performed -= OnCancel;
            inputControls.UI.Navigate.performed -= OnNavigate;
            inputControls.UI.Debug.performed -= OnDebug;
        }
        
        private void OnSubmit(InputAction.CallbackContext context)
        {
            dispatcher.Dispatch(DigitalInputActionType.Submit, false);
        }
        
        private void OnCancel(InputAction.CallbackContext context)
        {
            dispatcher.Dispatch(DigitalInputActionType.Cancel, false);
        }
        
        private void OnDebug(InputAction.CallbackContext context)
        {
            dispatcher.Dispatch(DigitalInputActionType.Debug, false);
        }
        
        private void OnNavigate(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            dispatcher.Dispatch(AxisInputActionType.Navigate, direction);
        }

        private void OnFireStart(InputAction.CallbackContext context)
        {
            dispatcher.Dispatch(DigitalInputActionType.FireGun, true);
        }
        
        private void OnFireEnd(InputAction.CallbackContext context)
        {
            dispatcher.Dispatch(DigitalInputActionType.FireGun, false);
        }
        
        private void OnFireWeapon(InputAction.CallbackContext context)
        {
            dispatcher.Dispatch(DigitalInputActionType.FireWeapon, false);
        }
        
        private void OnChangeWeapon(InputAction.CallbackContext context)
        {
            dispatcher.Dispatch(DigitalInputActionType.ChangeWeapon, false);
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            dispatcher.Dispatch(AxisInputActionType.Move, direction);
        }
        
        private void OnPause(InputAction.CallbackContext context)
        {
            dispatcher.Dispatch(DigitalInputActionType.Pause);
        }

        public override void Shutdown()
        {
            inputControls?.Player.Disable();
            inputControls?.UI.Disable();
            UnRegisterEvents();
        }
        
        public string SaveBindingOverridesAsJson()
        {
            return inputControls.SaveBindingOverridesAsJson();
        }
        
        public void RemoveAllBindingOverrides()
        {
            inputControls.RemoveAllBindingOverrides();
        }
        
        public void LoadBindingOverridesFromJson(string json)
        {
            inputControls.LoadBindingOverridesFromJson(json);
        }
    }
}