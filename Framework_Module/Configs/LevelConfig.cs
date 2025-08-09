using System;
using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Scriptable Objects/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private LevelDefinition[] levelDatas;
        public IReadOnlyList<LevelDefinition> LevelDatas => levelDatas;

        private void OnValidate()
        {
        }
    }
}