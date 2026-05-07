using System;
using Framework_Module.Event;
using Framework_Module.Event.Ui;
using Framework_Module.Game_State;
using Framework_Module.Service;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Application = UnityEngine.Application;

namespace Ui_Module
{
    public class MenuUi : MonoBehaviour
    {
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button exitButton;
        private EventBus eventBus;

        private void OnEnable()
        {
            if (EventSystem.current)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(newGameButton.gameObject);
            }
        }

        private void Start()
        {
            eventBus = Services.Get<EventBus>();
            newGameButton.onClick.AddListener(OnNewGame);
            exitButton.onClick.AddListener(OnExit);
        }

        private void OnDestroy()
        {
            newGameButton.onClick.RemoveListener(OnNewGame);
            exitButton.onClick.RemoveListener(OnExit);
        }

        private void OnExit()
        {
#if UNITY_STANDALONE
            Application.Quit();
#endif
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        
        private void OnNewGame()
        {
            eventBus.Publish(new NewGameSelectedEvent());
        }
    }
}
