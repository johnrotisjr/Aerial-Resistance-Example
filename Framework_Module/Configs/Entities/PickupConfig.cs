using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Configs.Entities
{
    [CreateAssetMenu(fileName = "PickupConfig", menuName = "Scriptable Objects/PickupConfig")]
    public class PickupConfig : ScriptableObject
    {
        [SerializeField] private PickupDefinition[] pickupData;
        [SerializeField] private GameObject pickupPrefab;
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