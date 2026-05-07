using System.Collections.Generic;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using Framework_Module.Event;
using UnityEngine;

namespace Ui_Module.Screens
{
    public class WeaponSelectionScreen : SelectionScreen<WeaponDefinition>
    {
        public override ScreenType ScreenType => ScreenType.Weapon;
        private void Awake()
        {
            AllowMultipleSelections = true;
        }

        protected override GameButtonSettings[] PopulateGameButtons()
        {
            var weaponData = ConfigDatabase.GetAllWeaponData();
            var gameButtonData = new List<GameButtonSettings>();
            var buttonId = 0;
            foreach (var definition in weaponData)
            {
                if (!definition.CanBePurchased)
                    continue;
                gameButtonData.Add(new GameButtonSettings(
                    buttonId,
                    ButtonTransitionType.Color,
                    definition.Icon,
                    null,
                    null,
                    null,
                    null,
                    null,
                    string.Empty,
                    true,
                    OnButtonClicked
                ));
                ButtonToDataMap.Add(buttonId, definition);
                buttonId++;
            }

            return gameButtonData.ToArray();
        }

        protected override void OnSubmit()
        {
            var selectedData = GetSelectedData();
            EventBus.Publish(new WeaponsSelectedEvent(selectedData)); 
        }
    }
}
