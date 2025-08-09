using System.Collections.Generic;
using Framework_Module.Enums;

namespace Framework_Module.Interfaces
{
    public interface IPersistentPlayerData
    {
        public int Cash { get; }
        public void AddCash(int cash);
        public IReadOnlyCollection<bool> CompletedLevels { get; }
        public void CompleteLevel(int levelId);
        public bool IsLevelCompleted(int index);
        public int UpdateVehicleUpgradeData(VehicleArchetype vehicleArchetype, UpgradeType upgradeType, int increment);
        public IUpgradeData GetVehicleUpgrades(VehicleArchetype archetype);
    }
}
