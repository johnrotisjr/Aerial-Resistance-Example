using Framework_Module.Definitions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework_Module.GameData.Data
{
    /// <summary>
    /// ScriptableObject that defines mission-specific data such as enemy spawns, mission type, and objectives.
    /// Used to configure gameplay scenarios.
    /// </summary>

    [CreateAssetMenu(fileName = "MissionData", menuName = "Scriptable Objects/MissionData")]
    public class MissionData : ScriptableObject
    {
        [SerializeField] public MissionDefinition definition;
        public void OnValidate()
        {
            //data.spawnInstructions.Sort((a, b)=> a.spawnDistance.CompareTo(b.spawnDistance));
        }
    }
}
