using System.Collections.Generic;
using System.ComponentModel;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using UnityEditor;
using UnityEngine;

namespace Framework_Module.GameData.Data
{
    [CreateAssetMenu(fileName = "VehicleData", menuName = "Scriptable Objects/VehicleData")]
    public class VehicleData : ScriptableObject
    {
        [SerializeField, ReadOnly(true)] private string guid;
        [SerializeField, ReadOnly(true)] private string readableId;
        [SerializeField] private VehicleDefinition vehicleDefinition;
        public string Guid => guid;
        public string ReadableId => readableId;
        public VehicleDefinition VehicleDefinition => vehicleDefinition;

        private void OnValidate()
        {
#if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(this);
            if (!string.IsNullOrEmpty(path))
            {
                string metaGuid = AssetDatabase.AssetPathToGUID(path);
                if (guid != metaGuid)
                {
                    guid = metaGuid;
                    EditorUtility.SetDirty(this);
                }
            }
            
            if (vehicleDefinition != null)
            {
                var parts = new List<string>(4);
                
                if (vehicleDefinition.Tier != VehicleTier.Normal)
                    parts.Add(vehicleDefinition.Tier.ToString());
                
                if (vehicleDefinition.Category != VehicleCategory.Generic)
                    parts.Add(vehicleDefinition.Category.ToString());
                
                parts.Add(vehicleDefinition.Archetype.ToString());
                
                if (vehicleDefinition.Role != VehicleRole.None)
                    parts.Add(vehicleDefinition.Role.ToString());

                parts.Add(guid.Substring(0,5));
                
                readableId = string.Join("_", parts);
                EditorUtility.SetDirty(this);
            }
#endif
        }
    }
}
