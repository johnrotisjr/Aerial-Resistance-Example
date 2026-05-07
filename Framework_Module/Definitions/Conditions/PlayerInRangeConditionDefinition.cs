using System;
using Framework_Module.Enums;
using UnityEngine;


namespace Framework_Module.Definitions
{
    [Serializable]
    public sealed class PlayerInRangeConditionDefinition : AiTransitionConditionDefinition
    {
        [Min(0f)] [SerializeField] private double distanceFromPlayer = 0;
        public double DistanceFromPlayer => distanceFromPlayer;

    }
}