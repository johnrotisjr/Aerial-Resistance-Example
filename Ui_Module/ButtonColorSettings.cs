using System;
using UnityEngine;

namespace Ui_Module
{
    [Serializable]
    public class ButtonColorSettings
    {
        public Color chosenColor;
        public Color normalColor;
        public Color highlightedColor;
        public Color pressedColor;
        public Color selectedColor;
        public Color disabledColor;

        public ButtonColorSettings(Color chosen, Color normal, Color highlighted, 
            Color pressed, Color selected, Color disabled)
        {
            chosenColor = chosen;
            normalColor = normal;
            highlightedColor = highlighted;
            pressedColor = pressed;
            selectedColor = selected;
            disabledColor = disabled;
        }
    }

}