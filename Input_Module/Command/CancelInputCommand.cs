using Framework_Module.Enums;
using Framework_Module.Interfaces;

namespace Input_Module.Command
{
    public class CancelInputCommand : IDigitalInputCommand
    {
        private readonly IUiInputHandler uiInputHandler;
        public CancelInputCommand(IUiInputHandler uiInputHandler)
        {
            this.uiInputHandler = uiInputHandler;
        }

        public void Execute(bool isPressed)
        {
            uiInputHandler.OnCancel();
        }
    }
}
