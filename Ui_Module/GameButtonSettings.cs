using System;
using Framework_Module.Enums;
using UnityEngine;

namespace Ui_Module
{
    [Serializable]
    public class GameButtonSettings
    {
        [SerializeField] public int buttonId;
        
        [SerializeField] private ButtonSpriteSettings backgroundButtonSpriteSettings;
        [SerializeField] private ButtonColorSettings backgroundButtonColorSettings;
        
        [SerializeField] private ButtonSpriteSettings foregroundButtonSpriteSettings;
        [SerializeField] private ButtonColorSettings foregroundButtonColorSettings;
        
        [SerializeField] private string text;
        [SerializeField] private bool isInteractable;
        [SerializeField] private ButtonTransitionType buttonTransitionType;
        [SerializeField] private Sprite defaultFgSprite;
        [SerializeField] private Sprite defaultBgSprite;
        
        public Sprite DefaultFgSprite => defaultFgSprite;
        public Sprite DefaultBgSprite => defaultBgSprite;
        public Action<int> OnButtonClicked;
        public int ButtonId => buttonId;
 
        public ButtonSpriteSettings BackgroundButtonSpriteSettings => backgroundButtonSpriteSettings;
        public ButtonColorSettings BackgroundButtonColorSettings => backgroundButtonColorSettings;
        
        public ButtonSpriteSettings ForegroundButtonSpriteSettings => foregroundButtonSpriteSettings;
        public ButtonColorSettings ForegroundButtonColorSettings => foregroundButtonColorSettings;
        
        public string Text => text;
        public bool IsInteractable => isInteractable;
        public ButtonTransitionType ButtonTransitionType => buttonTransitionType;

        public GameButtonSettings(
            int buttonId,
            ButtonTransitionType type,
            Sprite defaultFgSprite,
            Sprite defaultBgSprite,
            ButtonSpriteSettings backgroundButtonSpriteSettings,
            ButtonColorSettings backgroundButtonColorSettings,
            ButtonSpriteSettings foregroundButtonSpriteSettings,
            ButtonColorSettings foregroundButtonColorSettings,
            string text,
            bool isInteractable,
            Action<int> onClicked = null)
        {
            this.buttonId = buttonId;
            this.buttonTransitionType = type;
            this.defaultBgSprite = defaultBgSprite;
            this.defaultFgSprite = defaultFgSprite;
            this.backgroundButtonSpriteSettings = backgroundButtonSpriteSettings;
            this.backgroundButtonColorSettings = backgroundButtonColorSettings;
            this.foregroundButtonSpriteSettings = foregroundButtonSpriteSettings;
            this.foregroundButtonColorSettings = foregroundButtonColorSettings;
            this.text = text;
            this.isInteractable = isInteractable;
            this.OnButtonClicked = onClicked;
        }
    }
}