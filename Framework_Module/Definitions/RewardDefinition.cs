using System;
using Framework_Module.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct RewardDefinition
    {
        [FormerlySerializedAs("rewardCategory")] [SerializeField] private RewardType rewardType;
        [SerializeField] private float factor;
        [SerializeField] string displayName;
        public string DisplayName => displayName;
        public RewardType RewardType => rewardType;
        public float Factor => factor;

        public RewardDefinition(string displayName, float factor, RewardType rewardType)
        {
            this.displayName = displayName;
            this.factor = factor;
            this.rewardType = rewardType;
        }

    }
}