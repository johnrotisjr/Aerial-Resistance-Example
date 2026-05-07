using Framework_Module.Enums;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Input_Module.Command
{
 

    public class NavigateInputCommand : IAxisInputCommand
    {
        private readonly IUiInputHandler uiInputHandler;
        public NavigateInputCommand(IUiInputHandler uiInputHandler)
        {
            this.uiInputHandler = uiInputHandler;
        }

        public void Execute(Vector2 inputValue)
        {
            uiInputHandler.OnNavigate(inputValue);
        }

    }
}
