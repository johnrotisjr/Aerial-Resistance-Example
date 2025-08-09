using System;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions
{
    /// <summary>
    /// Represents a condition for transitioning between AI states (e.g., time-based).
    /// Used in phases, routines, and patterns to determine flow.
    /// </summary>

    [Serializable]
    public struct AiTransitionConditionDefinition
    {
        [SerializeField] private AiTransitionType type;
        [SerializeField] private float transitionValue;

        public Func<bool> TransitionCondition;

        public AiTransitionType Type => type;
        public float TransitionValue => transitionValue;

        public AiTransitionConditionDefinition(AiTransitionType type, float transitionValue, Func<bool> transitionCondition = null)
        {
            this.type = type;
            this.transitionValue = transitionValue;
            TransitionCondition = transitionCondition;
        }
    }
}