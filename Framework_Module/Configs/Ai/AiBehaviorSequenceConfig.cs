using System.Collections.Generic;
using Framework_Module.Definitions;
using UnityEngine;

namespace Framework_Module.Configs.Ai
{
    /// <summary>
    /// ScriptableObject containing the complete AI behavior tree for an Vehicle, including all phases and transition rules.
    /// </summary>

    [CreateAssetMenu(fileName = "AiBehaviorTree", menuName = "Scriptable Objects/Ai/BehaviorTree")]
    public class AiBehaviorSequenceConfig : ScriptableObject
    {
        [SerializeField] private List<AiPhaseDefinition> aiPhases;
        [SerializeField] private bool isLooped = true;

        public IReadOnlyList<AiPhaseDefinition> AiPhases => aiPhases;
        public bool IsLooped => isLooped;

    }
}
