using System.Collections.Generic;
using Framework_Module.GameData.Data;
using UnityEngine;

namespace Framework_Module.GameData.Databases
{

    [CreateAssetMenu(fileName = "MissionDatabase", menuName = "Scriptable Objects/MissionDatabase")]
    public class MissionDatabase : ScriptableObject
    {
        [SerializeField] private MissionData[] database;
        public IReadOnlyList<MissionData> Database => database;
        
        public void OnValidate()
        {
            
        }
    }
}
