 
using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using UnityEngine;
 

namespace Framework_Module.Configs.Entities
{
    /// <summary>
    /// Defines Vehicle configuration data including stats and AI behavior references for each plane type.
    /// Used to configure both player and enemy Vehicle.
    /// </summary>
    [CreateAssetMenu(fileName = "VehicleConfig", menuName = "Scriptable Objects/VehicleConfig")]
    public class VehicleConfig : ScriptableObject
    {
        [SerializeField] private VehicleDefinition[] vehicleData;
        public IReadOnlyList<VehicleDefinition> VehicleDatas => vehicleData;

        private void OnValidate()
        {
            HashSet<VehicleArchetype> archetypes = new HashSet<VehicleArchetype>();
            foreach (var d in vehicleData)
                if(!archetypes.Add(d.VehicleArchetype))
                    DebugLogger.Log("Duplicate Vehicle Archetypes found!", LogCategory.Framework, LogLevel.Error);
        }
    }
}
