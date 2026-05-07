using System;
using Debug_Module;
using Framework_Module.Enums;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input_Module
{
    /// <summary>
    /// Manages input bindings, rebinding operations, and persistence of input configurations.
    /// Supports saving and loading input schemes to and from JSON.
    /// </summary>
    
    public class InputBindingManager : IGameService
    {
        private const string RebindsKey = "InputBindings";
        private readonly IInputController inputController;

        public InputBindingManager(IInputController inputController)
        {
            this.inputController = inputController;
        }

        public void Initialize()
        {
            LoadBindings();
        }

        public void SaveBindings()
        {
            string json = inputController.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString(RebindsKey, json);
            PlayerPrefs.Save();
        }

        public void LoadBindings()
        {
            if (PlayerPrefs.HasKey(RebindsKey))
            {
                string json = PlayerPrefs.GetString(RebindsKey);
                inputController.LoadBindingOverridesFromJson(json);
            }
        }

        public void ResetBindings()
        {
            inputController.RemoveAllBindingOverrides();
            PlayerPrefs.DeleteKey(RebindsKey);
        }

        public void StartRebind(DigitalInputActionType inputActionType, Action onComplete = null)
        {
            var action = inputController.GetDigitalAction(inputActionType);
            if (action == null)
            {
                DebugLogger.Log($"Input action '{inputActionType.ToString()}' not found.", LogCategory.Input, LogLevel.Log);
                return;
            }
            
            StartInteractiveRebind(action, onComplete);
        }
        
        public void StartRebind(AxisInputActionType inputActionType, Action onComplete = null)
        {
            var action = inputController.GetAxisAction(inputActionType);
            if (action == null)
            {
                DebugLogger.Log($"Input action '{inputActionType.ToString()}' not found.", LogCategory.Input, LogLevel.Log);
                return;
            }

            StartInteractiveRebind(action, onComplete);
        }

        private void StartInteractiveRebind(InputAction action, Action onComplete = null)
        {
            action.Disable();
            var rebindOp = action.PerformInteractiveRebinding()
                .WithCancelingThrough("<Keyboard>/escape")
                .OnComplete(op =>
                {
                    action.Enable();
                    op.Dispose();
                    SaveBindings();
                    onComplete?.Invoke();
                })
                .Start();
        }

        public void Shutdown()
        {

            
        }
    }
}