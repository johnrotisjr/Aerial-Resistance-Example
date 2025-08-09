using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework_Module.Configs.Entities
{
    /// <summary>
    /// Stores weapon definitions such as speed, damage, and visual type for each weapon variant.
    /// Used for configuring player and enemy weapons.
    /// </summary>

    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Scriptable Objects/WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        [FormerlySerializedAs("projectileData")] [SerializeField] private WeaponDefinition[] weaponData;
 
        public IReadOnlyList<WeaponDefinition> WeaponDatas => weaponData;
        

        private void OnValidate()
        {
            HashSet<WeaponType> types = new HashSet<WeaponType>();
            foreach (var d in weaponData)
                if(!types.Add(d.Type))
                    DebugLogger.Log("Duplicate weapon type found!", LogCategory.Framework, LogLevel.Error);
        }
    }
}
