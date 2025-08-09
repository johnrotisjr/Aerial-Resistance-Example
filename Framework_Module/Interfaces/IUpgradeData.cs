using System.Collections.Generic;
using Framework_Module.Enums;

namespace Framework_Module.Interfaces
{
    public interface IUpgradeData
    {
        public IReadOnlyCollection<UpgradeLevel> Upgrades { get; }
        public UpgradeLevel GetVehicleUpgradeData(UpgradeType type);
        public int UpdateVehicleUpgradeData(UpgradeType type, int increment);
    }
}