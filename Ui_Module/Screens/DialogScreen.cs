using System.Collections.Generic;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui_Module.Screens
{
    public class DialogScreen : GameScreen
    {
        [SerializeField] private Image avatar;
        [SerializeField] private TMP_Text text;
        private IConfigDatabase configDatabase;
        private IGameData gameData;
        private EventBus eventBus;
        public override ScreenType ScreenType => ScreenType.Dialog;
        private Sprite[] avatars;
        private IReadOnlyList<DialogEntryDefinition> dialog;
        private int currentIndex;
        private IWorldStateManager worldStateManager;

        private void Awake()
        {
            configDatabase = Services.Get<IConfigDatabase>();
            gameData = Services.Get<IGameData>();
            eventBus = Services.Get<EventBus>();
            worldStateManager = Services.Get<IWorldStateManager>();
        }
        
        public override void DisplayScreen()
        {
            base.DisplayScreen();
  
            var dialogSequenceDefinition = configDatabase.GetDialogSequenceDefinition(
                gameData.TransientPlayerData.SelectedMissionIndex, 
                worldStateManager.CurrentState.WorldStateType
                );
            
            if (dialogSequenceDefinition.HasValue && dialogSequenceDefinition.Value.DialogSequence != null)
            {
                dialog = dialogSequenceDefinition.Value.DialogSequence;
                currentIndex = -1;
                Next();
            }
        }

        private bool Next()
        {
            currentIndex++;
            if (currentIndex >= dialog.Count)
            {
                currentIndex = 0;
                return false;
            }
            
            avatar.sprite = configDatabase.GetAvatarSprite(dialog[currentIndex].AvatarType);
            text.text = dialog[currentIndex].Text;
            
            return true;
        }

        public override void OnSubmit(bool isPressed)
        {
            base.OnSubmit(isPressed);
            if(!Next())
                HideScreen();
        }
    }
}