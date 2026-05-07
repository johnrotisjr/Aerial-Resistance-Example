using System;
using UnityEngine;

namespace Ui_Module
{
    [Serializable]
    public class ButtonSpriteSettings
    {
        public Sprite chosenSprite;
        public Sprite normalSprite;
        public Sprite highlightedSprite;
        public Sprite pressedSprite;
        public Sprite selectedSprite;
        public Sprite disabledSprite;

        public ButtonSpriteSettings(Sprite chosen, Sprite normal, Sprite highlighted, Sprite pressed, 
            Sprite selected, Sprite disabled)
        {
            chosenSprite = chosen;
            normalSprite = normal;
            highlightedSprite = highlighted;
            pressedSprite = pressed;
            selectedSprite = selected;
            disabledSprite = disabled;
        }
    }

}