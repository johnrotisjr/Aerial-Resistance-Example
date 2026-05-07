using System;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions.Behaviors.Movement
{
    [Serializable]
    public class WobbleMovementAiBehaviorDefinition : MovementAiBehaviorDefinition
    {
        public override AiMovementType MovementType => AiMovementType.Wobble;
        [SerializeField] private float frequency = 1f;
        [SerializeField] private float amplitude = 1f;
        [SerializeField][Range(0f,1f)] private float positiveLimitPercent = 1f;
        [SerializeField][Range(0f,1f)] private float negativeLimitPercent = 1f;
        [SerializeField] private bool flattenAtLimit = false;
        public bool FlattenAtLimit => flattenAtLimit;
        public float Frequency => frequency;
        public float Amplitude => amplitude;
        public float PositiveLimitPercent => positiveLimitPercent;
        public float NegativeLimitPercent => negativeLimitPercent;
    }
}