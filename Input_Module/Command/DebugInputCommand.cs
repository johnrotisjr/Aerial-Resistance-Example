using Framework_Module.Enums;
using Framework_Module.Interfaces;

namespace Input_Module.Command
{
    /// <summary>
    /// Command that triggers a PauseEvent when executed.
    /// Used for pausing the game via input.
    /// </summary>
    
    public class DebugInputCommand : IDigitalInputCommand
    {
        private readonly IUiInputHandler uiInputHandler;
        
        public DebugInputCommand(IUiInputHandler uiInputHandler)
        {
            this.uiInputHandler = uiInputHandler;
        }
        
        public void Execute(bool isPressed)
        {
            uiInputHandler.OnDebug();
        }

    }
}
