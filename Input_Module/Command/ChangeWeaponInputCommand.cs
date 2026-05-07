using Framework_Module.Enums;
using Framework_Module.Interfaces;

namespace Input_Module.Command
{
 
    public class ChangeWeaponInputCommand : IDigitalInputCommand
    {
        private readonly IPlayerController playerController;
        
        public ChangeWeaponInputCommand(IPlayerController playerController)
        {
            this.playerController = playerController;
        }
        
        public void Execute(bool isPressed)
        {
            playerController.ProcessInput(DigitalInputActionType.ChangeWeapon, isPressed);
        }
    }
}
