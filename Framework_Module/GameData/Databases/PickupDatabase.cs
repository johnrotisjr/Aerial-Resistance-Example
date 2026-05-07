using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.GameData.Databases
{
    [CreateAssetMenu(fileName = "PickupDatabase", menuName = "Scriptable Objects/PickupDatabase")]
    public class PickupDatabase : ScriptableObject
    {
        [SerializeField] private GameObject pickupPrefab;
        [SerializeField] private PickupDefinition[] pickupData;
        public GameObject PickupPrefab => pickupPrefab;
        public IReadOnlyList<PickupDefinition> PickupDatas => pickupData;
        
        private void OnValidate()
        {
            HashSet<PickupType> types = new HashSet<PickupType>();
            foreach (var d in pickupData)
                if(!types.Add(d.Type))
                    DebugLogger.Log("Duplicate Pickup type found!", LogCategory.Framework, LogLevel.Error);
        }
    }
}