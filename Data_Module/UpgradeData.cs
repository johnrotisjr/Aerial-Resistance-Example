using System.Collections.Generic;
using Framework_Module;
using Framework_Module.Enums;
using Framework_Module.Interfaces;

namespace Data_Module
{
    public class UpgradeData : IUpgradeData
    {
        private readonly Dictionary<UpgradeType, UpgradeLevel> upgrades = new();
        public IReadOnlyCollection<UpgradeLevel> Upgrades => upgrades.Values;

        public UpgradeLevel GetVehicleUpgradeData(UpgradeType type)
        {
            return upgrades[type];
        }
        
        public int UpdateVehicleUpgradeData(UpgradeType type, int increment)
        {
            if(!upgrades.ContainsKey(type))
                upgrades.Add(type, new UpgradeLevel(type, 0));
            
            upgrades[type].Level += increment;
            return upgrades[type].Level;
        }
    }
}