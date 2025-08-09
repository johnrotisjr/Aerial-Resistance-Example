using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Definitions;
using UnityEngine;

namespace Framework_Module.Configs
{
    [CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Scriptable Objects/UpgradeConfig")]
    public class UpgradeConfig : ScriptableObject
    {
        [SerializeField] private UpgradeDefinition[] upgradeDefinitions;
        public IReadOnlyCollection<UpgradeDefinition> UpgradeDefinitions => upgradeDefinitions;
        
        private void OnValidate()
        {
            foreach (var def in upgradeDefinitions)
            {
                if(def.CostPerLevel.Count != def.MaxLevel)
                    DebugLogger.Log("Cost per level list is not equal to max level!", LogCategory.Framework, LogLevel.Error);
            }
        }
    }
}