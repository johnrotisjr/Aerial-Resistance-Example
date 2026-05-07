using System;
using Framework_Module.Enums;
using UnityEngine;


namespace Framework_Module.Definitions
{
    [Serializable]
    public sealed class TimeConditionDefinition : AiTransitionConditionDefinition
    {
        [Min(0f)] [SerializeField] private float seconds = 0f;
        [Range(0f,1f)] [SerializeField] private float variancePercent = 0f;
        public float VariancePercent => variancePercent;
        public float Seconds => seconds;
    }
}