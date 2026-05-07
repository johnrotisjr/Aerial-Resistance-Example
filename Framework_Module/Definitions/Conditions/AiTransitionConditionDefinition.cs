using System;
using UnityEngine;


namespace Framework_Module.Definitions
{
    /// <summary>
    /// Represents a condition for transitioning between AI states (e.g., time-based).
    /// Used in phases, routines, and patterns to determine flow.
    /// </summary>
    
    [Serializable]
    public abstract class AiTransitionConditionDefinition
    {
        [SerializeField] private bool flipLogic = false;
        public bool FlipLogic => flipLogic;
    }
}