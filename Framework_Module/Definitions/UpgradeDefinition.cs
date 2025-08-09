using System;
using System.Collections.Generic;
using System.Linq;
using Framework_Module.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct UpgradeDefinition
    {
        [SerializeField] UpgradeType upgradeType;
        [SerializeField] string displayName;
        [SerializeField] Sprite icon;
        [SerializeField] int maxLevel;
        [SerializeField] int[] costPerLevel;

        public UpgradeType UpgradeType => upgradeType;
        public string DisplayName => displayName;
        public Sprite Icon => icon;
        public int MaxLevel => maxLevel;
        public IReadOnlyCollection<int> CostPerLevel => costPerLevel;

        public UpgradeDefinition(string displayName, UpgradeType upgradeType, Sprite icon, int maxLevel, int[] costPerLevel)
        {
            this.displayName = displayName;
            this.upgradeType = upgradeType;
            this.icon = icon;
            this.maxLevel = maxLevel;
            this.costPerLevel = costPerLevel;
        }
    }
}