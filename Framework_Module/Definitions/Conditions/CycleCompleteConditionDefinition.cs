using System;
using Framework_Module.Enums;
using UnityEngine;


namespace Framework_Module.Definitions
{
    [Serializable]
    public sealed class CycleCompleteConditionDefinition : AiTransitionConditionDefinition
    {
        [Min(0f)] [SerializeField] private int cycles = 0;
        public int Cycles => cycles;
    }
}