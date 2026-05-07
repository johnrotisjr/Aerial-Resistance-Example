using System.Collections.Generic;
using Debug_Module;
using Framework_Module.GameData.Data;
using UnityEngine;

namespace Framework_Module.GameData.Databases
{
    /// <summary>
    /// Defines Vehicle configuration data including stats and AI behavior references for each plane type.
    /// Used to configure both player and enemy Vehicle.
    /// </summary>
    [CreateAssetMenu(fileName = "VehicleDatabase", menuName = "Scriptable Objects/VehicleDatabase")]
    public class VehicleDatabase : ScriptableObject
    {
        [SerializeField] private VehicleData[] database;
        public IReadOnlyList<VehicleData> Database => database;

        private void OnValidate()
        {
            HashSet<string> archetypes = new HashSet<string>();
            foreach (var vehicleData in database)
                if(!archetypes.Add(vehicleData.Guid))
                    DebugLogger.Log($"Duplicate Vehicle Guid found! {vehicleData.Guid}", LogCategory.Framework, LogLevel.Error);
        }
    }
}
