using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Event;
using Framework_Module.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Ui_Module.Screens
{
    public abstract class SelectionScreen<T> : GameScreen
    {
        [SerializeField] protected GameButton submitButton;
        [SerializeField] protected SelectionGrid selectionGrid;
        protected bool IsSelectionRequired = false;
        protected readonly Dictionary<int, T> ButtonToDataMap = new();
        protected IConfigDatabase ConfigDatabase;
        protected GameButton[] GameButtons;
        protected bool AllowMultipleSelections = false;
        
        public void Inject(IConfigDatabase configDatabaseService, IScreenManager screenManagerService, EventBus eventBusService)
        {
            ConfigDatabase = configDatabaseService;
            GameScreenInject(screenManagerService, eventBusService);
        }

        public override void DisplayScreen()
        {
            base.DisplayScreen();

            submitButton.SetInteractable(!IsSelectionRequired);
            submitButton.OnButtonClicked += OnSubmit;
            if (EventSystem.current)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(submitButton.gameObject);
            }

            var gameButtonData = PopulateGameButtons();

            GameButtons = selectionGrid.AddElements(gameButtonData);
        }

        protected abstract GameButtonSettings[] PopulateGameButtons();

        protected virtual void OnButtonClicked(int id)
        {
            if (!ButtonToDataMap.ContainsKey(id))
            {
                DebugLogger.Log($"Could not find data for buttonId {id}", LogCategory.Ui, LogLevel.Warning);
                return;
            }

            bool selectionMade = false;
            foreach (var gb in GameButtons)
            {
                if (gb.GameButtonSettings.ButtonId == id)
                {
                    if (!gb.IsChosen)
                    {
                        selectionMade = true;
                        gb.SetChosen(true);   
                    }
                    else if(gb.IsChosen)
                        gb.SetChosen(false);

                    if (AllowMultipleSelections)
                        return;
                }
                else if(!AllowMultipleSelections && gb.IsChosen) 
                    gb.SetChosen(false);
            }

            if (IsSelectionRequired)
            {
                submitButton.SetInteractable(selectionMade);
            }
        }
        
        protected abstract void OnSubmit();

        protected T[] GetSelectedData()
        {
            var data = new List<T>();
            foreach (var gb in GameButtons)
            {
                if (!ButtonToDataMap.ContainsKey(gb.GameButtonSettings.ButtonId))
                {
                    DebugLogger.Log($"Could not find data for buttonId {gb.GameButtonSettings.ButtonId}", LogCategory.Ui, LogLevel.Warning);
                    return null;
                }
                if(gb.IsChosen)
                    data.Add(ButtonToDataMap[gb.GameButtonSettings.ButtonId]);
            }

            return data.ToArray();
        }

        public override void HideScreen()
        {
            base.HideScreen();
            if(submitButton)
                submitButton.OnButtonClicked -= OnSubmit;
            if(EventSystem.current)
                EventSystem.current.SetSelectedGameObject(null);
            if(GameButtons != null)
                selectionGrid.RemoveElements(GameButtons);
            GameButtons = null;
            ButtonToDataMap.Clear();
        }
    }
}
