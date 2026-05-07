using Framework_Module.Enums;
using Framework_Module.Game_State;
using Framework_Module.Interfaces;

namespace Input_Module.Command
{
    public class SubmitInputCommand : IDigitalInputCommand
    {
        private readonly IUiInputHandler uiInputHandler;
        public SubmitInputCommand(IUiInputHandler uiInputHandler)
        {
            this.uiInputHandler = uiInputHandler;
        }

        public void Execute(bool isPressed)
        {
            uiInputHandler.OnSubmit();
        }
    }
}
