using System;
using Framework_Module.Enums;
using UnityEngine;


namespace Framework_Module.Definitions
{
    [Serializable]
    public sealed class PlayerRelativeLocationConditionDefinition : AiTransitionConditionDefinition
    {
        [SerializeField] private DirectionType direction;
        [SerializeField] [Range(0f, 180f)] private float halfAngle;

        public DirectionType Direction => direction;
        public float HalfAngle => halfAngle;
    }
}