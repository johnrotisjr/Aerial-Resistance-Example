using System;
using Framework_Module.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct RewardDefinition
    {
        [SerializeField] private RewardType rewardType;
        [SerializeField] private float factor;
        public RewardType RewardType => rewardType;
        public float Factor => factor;

        public RewardDefinition(float factor, RewardType rewardType)
        {
            this.factor = factor;
            this.rewardType = rewardType;
        }

    }
}