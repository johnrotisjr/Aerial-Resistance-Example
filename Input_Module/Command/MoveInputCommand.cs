using Framework_Module.Enums;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Input_Module.Command
{
    /// <summary>
    /// Command that routes a movement direction vector to the player controller for physics processing.
    /// </summary>

    public class MoveInputCommand : IAxisInputCommand
    {
        private readonly IPlayerController playerController;

        public MoveInputCommand(IPlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void Execute(Vector2 inputValue)
        {
            playerController.ProcessInput(AxisInputActionType.Move, inputValue);
        }
    }
}
