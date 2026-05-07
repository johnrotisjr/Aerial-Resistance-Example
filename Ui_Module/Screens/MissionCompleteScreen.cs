using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.Gameplay;
using Framework_Module.Event.State;
using Framework_Module.Interfaces;
using TMPro;
using Ui_Module.HudObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ui_Module.Screens
{
    /// <summary>
    /// UI screen shown when a mission ends.
    /// Displays results and provides options to restart or return to the main menu.
    /// </summary>

    public class MissionCompleteScreen : GameScreen
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private MissionProgressUi missionProgressUi;
        [SerializeField] private Button restartButton;

        private EventBus eventBus;

        public override ScreenType ScreenType => ScreenType.MissionComplete;

        public void Inject(IScreenManager manager, IObjectiveManager objectiveManager, EventBus eventBusService, 
            IConfigDatabase configDatabaseService, IGameData gameDataService)
        {
            eventBus = eventBusService;
            GameScreenInject(manager, eventBus);
            missionProgressUi.Inject(eventBus, configDatabaseService, gameDataService, objectiveManager);
        }

        public override void DisplayScreen()
        {
            base.DisplayScreen();
            if (closeScreenButton)
            {
                closeScreenButton.onClick.RemoveAllListeners();
                closeScreenButton.onClick.AddListener(OnMissionHubReturn);
            }

            if(restartButton)
                restartButton.onClick.AddListener(OnRestartMission);
            
            eventBus?.Subscribe<GameOverEvent>(OnGameOver);
            if (EventSystem.current && closeScreenButton)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(closeScreenButton.gameObject);
            }
        }

        public override void HideScreen()
        {
            base.HideScreen();
            eventBus?.Unsubscribe<GameOverEvent>(OnGameOver);
            if(closeScreenButton)
                closeScreenButton.onClick.RemoveListener(OnMissionHubReturn);
            if(restartButton)
                restartButton.onClick.RemoveListener(OnRestartMission);
            
            if (EventSystem.current)
                EventSystem.current.SetSelectedGameObject(null);
        }

        private void OnRestartMission()
        {
            eventBus?.Publish(new RestartMissionEvent());
            Manager.CloseCurrentScreen();
        }
        
        private void OnMissionHubReturn()
        {
            eventBus?.Publish(new ExitMissionEvent());
        }

        private void OnGameOver(GameOverEvent e)
        {
            title.text = e.MissionComplete ? "Mission Complete!" : "GameOver";
        }
    }
}
