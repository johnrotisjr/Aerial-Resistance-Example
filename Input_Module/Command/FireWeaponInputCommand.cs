using Framework_Module.Enums;
using Framework_Module.Interfaces;

namespace Input_Module.Command
{
 
    
    public class FireWeaponInputCommand : IDigitalInputCommand
    {
        private readonly IPlayerController playerController;
        
        public FireWeaponInputCommand(IPlayerController playerController)
        {
            this.playerController = playerController;
        }
        
        public void Execute(bool isPressed)
        {
            playerController.ProcessInput(DigitalInputActionType.FireWeapon, isPressed);
        }
    }
}
