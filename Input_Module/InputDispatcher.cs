using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Enums;
using Framework_Module.Game_State;
using Framework_Module.Interfaces;
using Input_Module.Command;
using UnityEngine;

namespace Input_Module
{
    /// <summary>
    /// Centralized system that maps input action types to registered input command objects.
    /// Supports digital and axis input commands using a command pattern.
    /// </summary>

    public class InputDispatcher : IGameService
    {
        private readonly Dictionary<DigitalInputActionType, IDigitalInputCommand> digitalCommands = new();
        private readonly Dictionary<AxisInputActionType, IAxisInputCommand> axisCommands = new();
        private readonly IUiInputHandler uiInputHandler;
        private readonly IPlayerController playerController;
        
        public InputDispatcher(IUiInputHandler uiInputHandler, IPlayerController playerController)
        {
            this.uiInputHandler = uiInputHandler;
            this.playerController = playerController;
        }
        
        private void RegisterAllInput()
        {
            RegisterUiInput();
            RegisterPlayerInput();
        }

        private void RegisterPlayerInput()
        {
            RegisterAxisCommand(AxisInputActionType.Move, new MoveInputCommand(playerController));
            RegisterDigitalCommand(DigitalInputActionType.FireGun, new FireGunInputCommand(playerController));
            RegisterDigitalCommand(DigitalInputActionType.FireWeapon, new FireWeaponInputCommand(playerController));
            RegisterDigitalCommand(DigitalInputActionType.ChangeWeapon, new ChangeWeaponInputCommand(playerController));
        }

        private void RegisterUiInput()
        {
            RegisterAxisCommand(AxisInputActionType.Navigate, new NavigateInputCommand(uiInputHandler));
            RegisterDigitalCommand(DigitalInputActionType.Submit, new SubmitInputCommand(uiInputHandler));
            RegisterDigitalCommand(DigitalInputActionType.Cancel, new CancelInputCommand(uiInputHandler));
            RegisterDigitalCommand(DigitalInputActionType.Pause, new PauseInputCommand(uiInputHandler));
            RegisterDigitalCommand(DigitalInputActionType.Debug, new DebugInputCommand(uiInputHandler));
        }

        private void UnregisterAllInput()
        {
            UnregisterPlayerInput();
            UnregisterUiInput();
        }
        
        private void UnregisterUiInput()
        {
            UnregisterAxisCommand(AxisInputActionType.Navigate);
            UnregisterDigitalCommand(DigitalInputActionType.Submit);
            UnregisterDigitalCommand(DigitalInputActionType.Cancel);
            UnregisterDigitalCommand(DigitalInputActionType.Pause);
            UnregisterDigitalCommand(DigitalInputActionType.Debug);
        }
        
        private void UnregisterPlayerInput()
        {
            UnregisterAxisCommand(AxisInputActionType.Move);
            UnregisterDigitalCommand(DigitalInputActionType.FireGun);
            UnregisterDigitalCommand(DigitalInputActionType.FireWeapon);
            UnregisterDigitalCommand(DigitalInputActionType.ChangeWeapon);
        }

        public void Initialize()
        {
            RegisterAllInput();
        }
        
        public void Shutdown()
        {
            UnregisterAllInput();
        }
        
        private void RegisterDigitalCommand(DigitalInputActionType action, IDigitalInputCommand command)
        {
            digitalCommands[action] = command;
        }

        private void RegisterAxisCommand(AxisInputActionType action, IAxisInputCommand command)
        {
            axisCommands[action] = command;
        }

        private void UnregisterDigitalCommand(DigitalInputActionType action)
        {
            digitalCommands.Remove(action);
        }
        
        private void UnregisterAxisCommand(AxisInputActionType action)
        {
            axisCommands.Remove(action);
        }

        public void Dispatch(AxisInputActionType action, Vector2 axis)
        {
            if (axisCommands.TryGetValue(action, out var command))
            {
                command.Execute(axis);
            }
            else
            {
                DebugLogger.Log($"No axis command registered for {action}", LogCategory.Input, LogLevel.Warning);
            }
        }
        
        public void Dispatch(DigitalInputActionType action, bool isPressed = true)
        {
            if (digitalCommands.TryGetValue(action, out var command))
            {
                command.Execute(isPressed);
            }
            else
            {
                DebugLogger.Log($"No button command registered for {action}", LogCategory.Input, LogLevel.Warning);
            }
        }
    }
}
