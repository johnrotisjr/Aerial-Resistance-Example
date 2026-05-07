using Debug_Module;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.Ui;
using Framework_Module.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Ui_Module.Screens
{
    /// <summary>
    /// Abstract base class for UI screens with lifecycle support (Show, Hide, Refresh).
    /// Used for menus, overlays, and modals.
    /// </summary>

    [RequireComponent(typeof(CanvasGroup))]
    public abstract class GameScreen : MonoBehaviour, IGameScreen
    {
        private CanvasGroup canvasGroup;
        [SerializeField] protected Button closeScreenButton;
        protected IScreenManager Manager;
        protected EventBus EventBus;

        public abstract ScreenType ScreenType { get; }

        //TODO: This is messy and error prone, lets find a better way to do this
        public void GameScreenInject(IScreenManager screenManagerService, EventBus eventBusService)
        {
            Manager = screenManagerService;
            EventBus = eventBusService;
        }

        protected virtual void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if(!canvasGroup)
            {
                DebugLogger.Log("canvasGroup is Required, but could not be found!", LogCategory.Ui, LogLevel.Error);
                return;
            }
            
            HideScreen();
            if(closeScreenButton)
                closeScreenButton.onClick.AddListener(CloseCurrentScreen);
        }

        private void CloseCurrentScreen()
        {
            if(Manager != null)
                Manager.CloseCurrentScreen();
        }

        protected virtual void OnDestroy()
        {
            if(closeScreenButton)
                closeScreenButton.onClick.RemoveAllListeners();
        }

        public virtual void DisplayScreen()
        {
            if (canvasGroup)
            {
                canvasGroup.alpha = 1;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.interactable = true;
            }

            EventBus.Publish(new DisplayScreenEvent(this));
        }
        
        public virtual void HideScreen()
        {
            if (canvasGroup)
            {
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
            }

            EventBus.Publish(new HideScreenEvent(this));
        }
        
        public virtual void RefreshScreen()
        {
            
        }

        public virtual void OnSubmit(bool isPressed)
        {
            
        }

        public virtual void OnNavigate(Vector2 inputValue)
        {
            
        }
    }
}
