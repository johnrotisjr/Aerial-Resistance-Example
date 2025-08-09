using System;
using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Configs
{
    [CreateAssetMenu(fileName = "RewardConfig", menuName = "Scriptable Objects/RewardConfig")]
    public class RewardConfig : ScriptableObject
    {
        [SerializeField] private RewardDefinition[] rewardFactors;
        public IReadOnlyList<RewardDefinition> RewardFactors => rewardFactors;
        
        [SerializeField] private List<int> missionRewardBaseValues;
        public IReadOnlyList<int> MissionRewardBaseValues => missionRewardBaseValues;

        private void OnValidate()
        {
            HashSet<RewardType> rewardCategories = new();
            foreach (var rewardFactor in rewardFactors)
            {
                if(!rewardCategories.Add(rewardFactor.RewardType))
                    DebugLogger.Log("Duplicate RewardCategory found", LogCategory.Framework, LogLevel.Warning);
            }
        }
    }
}