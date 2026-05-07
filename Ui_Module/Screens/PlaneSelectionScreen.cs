using System.Collections.Generic;
 
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.GameData.Data;
using UnityEngine;

namespace Ui_Module.Screens
{
    public class PlaneSelectionScreen : SelectionScreen<VehicleData>
    {
        [SerializeField] private VehicleInfoPanel vehicleInfoPanel;
        
        public override ScreenType ScreenType => ScreenType.Plane;                                                                                                                                                                                           

        protected void Awake()
        {
            IsSelectionRequired = true;
        }

        protected override GameButtonSettings[] PopulateGameButtons()
        {
            var vehicleDatas = ConfigDatabase.GetAllVehicleDefinition();
            var gameButtonData = new List<GameButtonSettings>();
            var buttonId = 0;
            foreach (var vehicleData in vehicleDatas)
            {
                if (!vehicleData.VehicleDefinition.IsPurchasable)
                    continue;
                gameButtonData.Add(new GameButtonSettings(
                    buttonId,
                    ButtonTransitionType.Color,
                    vehicleData.VehicleDefinition.Icon,
                    null,
                    null,
                    null,
                    null,
                    null,
                    string.Empty,
                    true,
                    OnButtonClicked
                ));
                ButtonToDataMap.Add(buttonId, vehicleData);
                buttonId++;
            }

            return gameButtonData.ToArray();
        }

        protected override void OnButtonClicked(int id)
        {
            base.OnButtonClicked(id);
            if (!ButtonToDataMap.TryGetValue(id, out var config))
            {
                return;
            }
            vehicleInfoPanel.Set(config);
        }

        protected override void OnSubmit()
        {
            EventBus.Publish(new PlaneSelectedEvent(GetSelectedData()[0])); 
        }
    }
}
