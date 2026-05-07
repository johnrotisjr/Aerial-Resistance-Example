using System;
using System.Collections.Generic;
using System.Data.Common;
using Debug_Module;
using Framework_Module;
using Framework_Module.Enums;
using Framework_Module.Interfaces;

namespace Data_Module
{
    /// <summary>
    /// Stores player-related runtime data such as currency, upgrades, and unlocks.
    /// Used for serialization and progression tracking.
    /// </summary>

    [Serializable]
    public class PersistentPlayerData : IPersistentPlayerData
    {
        public int Cash { get; private set; }
        public IReadOnlyCollection<bool> CompletedLevels => completedLevels;
        public bool IsLevelCompleted(int index) => completedLevels != null && index < completedLevels.Length && completedLevels[index]; 
        private bool[] completedLevels = new bool[100];//TODO: use real level count
        private Dictionary<string, IUpgradeData> upgradeData = new();

        public void CompleteLevel(int levelId)
        {
            if(levelId < 0 || levelId >= completedLevels.Length)
                DebugLogger.Log($"Index {levelId} is Out of bounds");
            
            completedLevels[levelId] = true;
        }
        
        public int UpdateVehicleUpgradeData(string id, UpgradeType upgradeType, int increment)
        {
            if(!upgradeData.ContainsKey(id))
                upgradeData.Add(id, new UpgradeData());

            var upgrade = upgradeData[id];
            return upgrade.UpdateVehicleUpgradeData(upgradeType, increment);
        }

        public IUpgradeData GetVehicleUpgrades(string id) => upgradeData.GetValueOrDefault(id);

        public PersistentPlayerData(int levelCount, int startingCash = 0)
        {
            Cash = startingCash;

            if (levelCount <= 0)
                return;
            
            completedLevels = new bool[levelCount];
        }
        
        public void AddCash(int cash)
        {
            Cash += cash;
        }

        public void CompleteLevel(uint levelIndex)
        {
            if (levelIndex >= completedLevels.Length)
            {
                DebugLogger.Log("Level index is out of bounds!", LogCategory.Data, LogLevel.Warning);
                return;
            }
            completedLevels[levelIndex] = true;
        }
        
    }
}
