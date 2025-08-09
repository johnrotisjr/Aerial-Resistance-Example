using Framework_Module.Definitions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework_Module.Configs
{
    /// <summary>
    /// ScriptableObject that defines mission-specific data such as enemy spawns, mission type, and objectives.
    /// Used to configure gameplay scenarios.
    /// </summary>

    [CreateAssetMenu(fileName = "MissionConfig", menuName = "Scriptable Objects/MissionConfig")]
    public class MissionConfig : ScriptableObject
    {
        [FormerlySerializedAs("data")] [SerializeField] public MissionDefinition definition;
        public void OnValidate()
        {
            //data.spawnInstructions.Sort((a, b)=> a.spawnDistance.CompareTo(b.spawnDistance));
        }
    }
}
