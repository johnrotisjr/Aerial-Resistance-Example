using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.State;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ui_Module.Screens
{
    public class MapScreen : GameScreen
    {
        public override ScreenType ScreenType => ScreenType.Map;
        [SerializeField] private GameButton[] levelButtons;

        private IGameData gameData;
        private IConfigDatabase config;

        private void Awake()
        {
            gameData = Services.Get<IGameData>();
            config = Services.Get<IConfigDatabase>();
        }

        public override void DisplayScreen()
        {
            base.DisplayScreen();

            for (var buttonIndex = 0; buttonIndex < levelButtons.Length; buttonIndex++)
            {
                var capInt = buttonIndex;
                levelButtons[buttonIndex].OnButtonClicked += () => OnSelection(capInt);
                var foundData = config.GetLevelDefinition(buttonIndex, out var data);
                if (foundData)
                {
                    var requirementsMet = true;
                    foreach (var levelIndex in data.UnlockRequirementsIndices)
                    {
                        if (gameData.PersistentPlayerData.IsLevelCompleted(levelIndex)) 
                            continue;
                        
                        requirementsMet = false;
                        break;
                    }
                    levelButtons[buttonIndex].SetInteractable(requirementsMet);
                }
                else
                {
                    levelButtons[buttonIndex].SetInteractable(false);
                }
            }

            closeScreenButton.onClick.AddListener(GoBack);
            if (EventSystem.current && levelButtons.Length > 0)
            {
                EventSystem.current.SetSelectedGameObject(null);
                if(levelButtons[0])
                    EventSystem.current.SetSelectedGameObject(levelButtons[0].gameObject);
            }
        }

        public override void HideScreen()
        {
            base.HideScreen();
            
            foreach (var b in levelButtons)
            {
                b.RemoveAllListeners();
            }
            closeScreenButton.onClick.RemoveListener(GoBack);
            if (EventSystem.current)
                EventSystem.current.SetSelectedGameObject(null);
        }

        private void GoBack()
        {
            HideScreen();
            EventBus.Publish(new GoBackEvent());
        }

        private void OnSelection(int index)
        {
            EventBus.Publish(new MissionSelectedEvent(index));
        }
    }
}
