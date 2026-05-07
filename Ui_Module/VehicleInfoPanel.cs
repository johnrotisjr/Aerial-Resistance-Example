using Debug_Module;
 
using Framework_Module.Enums;
using Framework_Module.GameData.Data;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using TMPro;
using UnityEngine;

namespace Ui_Module
{
    public class VehicleInfoPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelNumber;
        [SerializeField] private TMP_Text vehicleName;
        [SerializeField] private GameButton upgradeButton;
        private IGameData gameData;
        private IConfigDatabase configDatabase;
        private VehicleData vehicleData;

        public void Awake()
        {
            gameData = Services.Get<IGameData>();
            configDatabase = Services.Get<IConfigDatabase>();
            upgradeButton.OnButtonClicked += OnButtonClicked;
            levelNumber.text = "?";
        }

        private void OnButtonClicked()
        {
            var upgrade = gameData.PersistentPlayerData.GetVehicleUpgrades(vehicleData.Guid);
            int currentLevel = vehicleData.VehicleDefinition.Armor + (upgrade == null ? 0 : upgrade.GetVehicleUpgradeData(UpgradeType.Armor).Level);
            configDatabase.GetUpgradeDefinition(UpgradeType.Armor, out var upgradeDefinition);
            if (currentLevel + 1 > upgradeDefinition.MaxLevel)
            {
                DebugLogger.Log("Cannot upgrade, Already upgraded to max level", LogCategory.Ui, LogLevel.Warning);
            }
            else
            {
                currentLevel = gameData.PersistentPlayerData.UpdateVehicleUpgradeData(vehicleData.Guid, UpgradeType.Armor, 1);
            }
            
            levelNumber.text = currentLevel.ToString();
        }

        public void Set(VehicleData data)
        {
            vehicleData = data;
            vehicleName.text = vehicleData.VehicleDefinition.DisplayName;
            var upgradeGroup = gameData.PersistentPlayerData.GetVehicleUpgrades(vehicleData.Guid);
            if (!configDatabase.GetVehicleData(vehicleData.Guid, out var vehicleDefinition))
                return;
            
            if(upgradeGroup == null)
            {
                levelNumber.text = vehicleData.VehicleDefinition.Armor.ToString();
                return;
            }
            
            foreach (var upgradeLevel in upgradeGroup.Upgrades)
            {
                if (upgradeLevel.Type == UpgradeType.Armor)
                {
                    levelNumber.text = (vehicleData.VehicleDefinition.Armor + upgradeLevel.Level).ToString();
                    break;
                }
            }
        }
    }
}
