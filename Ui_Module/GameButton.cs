using System;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace Ui_Module
{
    [RequireComponent(typeof(Button))]
    public class GameButton : MonoBehaviour, IPooled<GameButtonSettings>, IPointerEnterHandler, IPointerExitHandler, 
        IPointerDownHandler, IPointerUpHandler
    {
        private enum ButtonState
        {
            Normal,
            Highlighted,
            Pressed,
            Selected,
            Disabled
        }
        
        [SerializeField] private AudioSfxType clickSoundType = AudioSfxType.ButtonClick;
        [SerializeField] private GameButtonSettings defaultGameButtonSettings;
        private GameButtonSettings overrideGameButtonSettings;
        
        [Header("Components")]
        [SerializeField] private Image bgImage;
        [SerializeField] private Image fgImage;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button button;
        
        private ButtonState state;
        private EventBus eventBus;
        
        private bool isChosen = false;
        public bool IsChosen => isChosen;
        public GameButtonSettings GameButtonSettings => overrideGameButtonSettings ?? defaultGameButtonSettings;
        public delegate void ButtonClicked();
        public event ButtonClicked OnButtonClicked;

        protected void Awake()
        {
            button = GetComponent<Button>();
            eventBus = Services.Get<EventBus>();
            bgImage.material = new Material(bgImage.material);
            SetFocusState(ButtonState.Normal);
            button.onClick.AddListener(OnClicked);
        }

        public void RemoveAllListeners()
        {
            OnButtonClicked = null;
        }

        public void Set(GameButtonSettings settings)
        {
            overrideGameButtonSettings = settings;

            if (settings.DefaultFgSprite != null)
            {
                fgImage.sprite = settings.DefaultFgSprite;
            }
            
            if (settings.DefaultBgSprite != null)
            {
                bgImage.sprite = settings.DefaultBgSprite;
            }
            
            bgImage.gameObject.SetActive(true);
            fgImage.gameObject.SetActive(true);
            fgImage.raycastTarget = false;
            
            if(text)
                text.SetText(settings.Text);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClicked);
            SetInteractable(settings.IsInteractable);
            transform.localScale = Vector3.one;
            SetFocusState(ButtonState.Normal);
        }

        private void OnClicked()
        {
            OnButtonClicked?.Invoke();
            GameButtonSettings.OnButtonClicked?.Invoke(GameButtonSettings.ButtonId);
            eventBus.Publish(new PlaySfxEvent(clickSoundType));
        }

        public void Clear()
        {
            bgImage.sprite = defaultGameButtonSettings.DefaultBgSprite;
            fgImage.sprite = defaultGameButtonSettings.DefaultFgSprite;
            
            if(text)
                text.SetText(string.Empty);
            button.onClick.RemoveAllListeners();
        }

        public void SetChosen(bool b)
        {
            isChosen = b;
            if (isChosen)
            {
                if (GameButtonSettings.ButtonTransitionType == ButtonTransitionType.Sprite)
                {
                    var backSettings = GameButtonSettings.BackgroundButtonSpriteSettings ?? defaultGameButtonSettings.BackgroundButtonSpriteSettings;
                    var foreSettings = GameButtonSettings.ForegroundButtonSpriteSettings ?? defaultGameButtonSettings.ForegroundButtonSpriteSettings;
                    bgImage.sprite = backSettings.chosenSprite;
                    fgImage.sprite = foreSettings.chosenSprite;
                }
                else if(GameButtonSettings.ButtonTransitionType == ButtonTransitionType.Color)
                {
                    var backSettings = GameButtonSettings.BackgroundButtonColorSettings ?? defaultGameButtonSettings.BackgroundButtonColorSettings;
                    var foreSettings = GameButtonSettings.ForegroundButtonColorSettings ?? defaultGameButtonSettings.ForegroundButtonColorSettings;
                    bgImage.color = backSettings.chosenColor;
                    fgImage.color = foreSettings.chosenColor;
                }
            }
            else
            {
                SetFocusState(ButtonState.Normal);
            }
        }
        
        private void SetFocusState(ButtonState newState)
        {
            state = newState;
            switch (GameButtonSettings.ButtonTransitionType)
            {
                case ButtonTransitionType.None:
                    break;
                case ButtonTransitionType.Sprite:
                    SetButtonTransitionSprites();
                    break;
                case ButtonTransitionType.Color:
                    SetButtonTransitionColors();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void SetButtonTransitionSprites()
        {
            var backgroundButtonSpriteSettings = GameButtonSettings.BackgroundButtonSpriteSettings ?? defaultGameButtonSettings.BackgroundButtonSpriteSettings;
            var foregroundButtonSpriteSettings = GameButtonSettings.ForegroundButtonSpriteSettings ?? defaultGameButtonSettings.ForegroundButtonSpriteSettings;
            
            Sprite backSprite;
            Sprite foreSprite;
            
            switch (state)
            {
                case ButtonState.Normal:
                    backSprite = backgroundButtonSpriteSettings.normalSprite;
                    foreSprite = foregroundButtonSpriteSettings.normalSprite;
                    break;
                case ButtonState.Highlighted:
                    backSprite = backgroundButtonSpriteSettings.highlightedSprite;
                    foreSprite = foregroundButtonSpriteSettings.highlightedSprite;
                    break;
                case ButtonState.Pressed:
                    backSprite = backgroundButtonSpriteSettings.pressedSprite;
                    foreSprite = foregroundButtonSpriteSettings.pressedSprite;
                    break;
                case ButtonState.Selected:
                    backSprite = backgroundButtonSpriteSettings.selectedSprite;
                    foreSprite = foregroundButtonSpriteSettings.selectedSprite;
                    break;
                case ButtonState.Disabled:
                    backSprite = backgroundButtonSpriteSettings.disabledSprite;
                    foreSprite = foregroundButtonSpriteSettings.disabledSprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
            
            if(bgImage) 
                bgImage.sprite = backSprite;
            if(fgImage) 
                fgImage.sprite = foreSprite;
        }
                
        private void SetButtonTransitionColors()
        {
            var backgroundButtonColorSettings = GameButtonSettings.BackgroundButtonColorSettings ?? defaultGameButtonSettings.BackgroundButtonColorSettings;
            var foregroundButtonColorSettings = GameButtonSettings.ForegroundButtonColorSettings ?? defaultGameButtonSettings.ForegroundButtonColorSettings;
            
            Color backColor;
            Color foreColor;
            
            switch (state)
            {
                case ButtonState.Normal:
                    backColor = backgroundButtonColorSettings.normalColor;
                    foreColor = foregroundButtonColorSettings.normalColor;
                    break;
                case ButtonState.Highlighted:
                    backColor = backgroundButtonColorSettings.highlightedColor;
                    foreColor = foregroundButtonColorSettings.highlightedColor;
                    break;
                case ButtonState.Pressed:
                    backColor = backgroundButtonColorSettings.pressedColor;
                    foreColor = foregroundButtonColorSettings.pressedColor;
                    break;
                case ButtonState.Selected:
                    backColor = backgroundButtonColorSettings.selectedColor;
                    foreColor = foregroundButtonColorSettings.selectedColor;
                    break;
                case ButtonState.Disabled:
                    backColor = backgroundButtonColorSettings.disabledColor;
                    foreColor = foregroundButtonColorSettings.disabledColor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
            
            if (bgImage) 
                bgImage.color = backColor;
            if(fgImage) 
                fgImage.color = foreColor;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!button.interactable || isChosen) 
                return;
            SetFocusState(ButtonState.Highlighted);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!button.interactable || isChosen) 
                return;
            SetFocusState(ButtonState.Normal);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!button.interactable || isChosen) 
                return;
            SetFocusState(ButtonState.Pressed);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!button.interactable || isChosen) 
                return;
            SetFocusState(ButtonState.Highlighted);
        }

        public void SetInteractable(bool isInteractable)
        {
            if (button == null)
                return;
            
            button.interactable = isInteractable;
            SetChosen(false);
            SetFocusState(isInteractable ? ButtonState.Normal : ButtonState.Disabled);
        }
    }
}
