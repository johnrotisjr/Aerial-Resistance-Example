using System;
using Framework_Module.Enums;
using UnityEngine;


namespace Framework_Module.Definitions
{
    [Serializable]
    public sealed class HealthPercentBelowConditionDefinition : AiTransitionConditionDefinition
    {
        [Range(0f, 1.0f)] [SerializeField] private float percent = 0;
        public float Percent => percent;
    }
}