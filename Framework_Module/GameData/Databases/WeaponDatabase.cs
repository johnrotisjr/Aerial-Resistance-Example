using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.GameData.Databases
{
    /// <summary>
    /// Stores weapon definitions such as speed, damage, and visual type for each weapon variant.
    /// Used for configuring player and enemy weapons.
    /// </summary>

    [CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Scriptable Objects/WeaponDatabase")]
    public class WeaponDatabase : ScriptableObject
    {
        [SerializeField] private WeaponDefinition[] database;
 
        public IReadOnlyList<WeaponDefinition> Database => database;
        

        private void OnValidate()
        {
            HashSet<WeaponType> types = new HashSet<WeaponType>();
            foreach (var d in database)
                if(!types.Add(d.Type))
                    DebugLogger.Log("Duplicate weapon type found!", LogCategory.Framework, LogLevel.Error);
        }
    }
}
