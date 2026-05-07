using System;
 
using Framework_Module.Core;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Interfaces;
using Ui_Module.HudObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Ui_Module.Screens
{
    /// <summary>
    /// UI screen shown when the game is paused.
    /// Allows the player to resume gameplay or access settings.
    /// </summary>

    public class PauseScreen : GameScreen
    {
        [SerializeField] private MissionProgressUi missionProgressUi;
        
        public override ScreenType ScreenType => ScreenType.Pause;

        public void Inject(IScreenManager manager, IObjectiveManager objectiveManager, EventBus eventBus, 
            IConfigDatabase configDatabaseService, IGameData gameData)
        {
            GameScreenInject(manager, eventBus);
            missionProgressUi.Inject(eventBus, configDatabaseService, gameData, objectiveManager);
        }

        public override void DisplayScreen()
        {
            base.DisplayScreen();
            missionProgressUi.UpdateUi();
            EventSystem.current.SetSelectedGameObject(null);
            if (closeScreenButton)
                EventSystem.current.SetSelectedGameObject(closeScreenButton.gameObject);
        }
    }
}
