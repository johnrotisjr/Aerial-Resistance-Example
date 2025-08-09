using System.Collections.Generic;
using Framework_Module.Definitions;
using UnityEngine;

namespace Framework_Module.Configs.Ai
{
    /// <summary>
    /// Represents a phase in the AI behavior tree, containing one or more routines.
    /// Defines looping and transition logic between phases.
    /// </summary>

    [CreateAssetMenu(fileName = "AiPhase", menuName = "Scriptable Objects/Ai/Phase")]
    public class AiPhaseConfig : ScriptableObject
    {
        [SerializeField] private List<AiRoutineDefinition> routines = new();
        [SerializeField] private bool isLooped = true;

        public IReadOnlyList<AiRoutineDefinition> Routines => routines;
        public bool IsLooped => isLooped;
    }
}
