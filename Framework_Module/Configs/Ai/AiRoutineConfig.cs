using System.Collections.Generic;
using Framework_Module.Definitions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework_Module.Configs.Ai
{
    /// <summary>
    /// Represents a routine in the AI behavior tree, containing a list of patterns and looping logic.
    /// </summary>

    [CreateAssetMenu(fileName = "AiRoutine", menuName = "Scriptable Objects/Ai/Routine")]
    public class AiRoutineConfig : ScriptableObject
    {
        [FormerlySerializedAs("patterns")] [SerializeField] private List<AiPatternPairDefinition> patternPairs = new();
        [SerializeField] private bool isLooped = true;

        public IReadOnlyList<AiPatternPairDefinition> PatternPairs => patternPairs;
        public bool IsLooped => isLooped;
    }
}
