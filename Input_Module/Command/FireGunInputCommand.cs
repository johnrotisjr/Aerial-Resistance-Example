using Framework_Module.Enums;
using Framework_Module.Interfaces;

namespace Input_Module.Command
{
    /// <summary>
    /// Command that triggers a FireGunEvent when executed.
    /// Represents the firing action mapped to a button press.
    /// </summary>
    
    public class FireGunInputCommand : IDigitalInputCommand
    {
        private readonly IPlayerController playerController;
        
        public FireGunInputCommand(IPlayerController playerController)
        {
            this.playerController = playerController;
        }
        
        public void Execute(bool isPressed)
        {
            playerController.ProcessInput(DigitalInputActionType.FireGun, isPressed);//Handle end and start gun fire
        }
    }
}
