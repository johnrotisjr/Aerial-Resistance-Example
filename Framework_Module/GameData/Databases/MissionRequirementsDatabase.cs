using System.Collections.Generic;
using Framework_Module.Definitions;
using UnityEngine;

namespace Framework_Module.GameData.Databases
{
    [CreateAssetMenu(fileName = "MissionRequirementsDatabase", menuName = "Scriptable Objects/MissionRequirementsDatabase")]
    public class MissionRequirementsDatabase : ScriptableObject
    {
        [SerializeField] private LevelDefinition[] database;
        public IReadOnlyList<LevelDefinition> Database => database;

        private void OnValidate()
        {
        }
    }
}