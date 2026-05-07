using System.Collections.Generic;
using Framework_Module.Definitions;
using Framework_Module.Definitions.BehaviorGroup;
using UnityEngine;

namespace Framework_Module.GameData.Ai
{
    /// <summary>
    /// Represents a routine in the AI behavior tree, containing a list of patterns and looping logic.
    /// </summary>

    [CreateAssetMenu(fileName = "AiRoutine", menuName = "Scriptable Objects/Ai/Routine")]
    public class AiRoutineConfig : ScriptableObject
    {
        [SerializeField] private List<IndependentAttackBehaviorDefinition> independentAttackBehaviors;
        [SerializeField] private List<AiBehaviorGroupDefinition> aiBehaviorGroups = new();
        [SerializeField] private bool isLooped = true;
        
        public IReadOnlyList<AiBehaviorGroupDefinition> AiBehaviorGroups => aiBehaviorGroups;
        public IReadOnlyList<IndependentAttackBehaviorDefinition> IndependentAttackBehaviors => independentAttackBehaviors;
        public bool IsLooped => isLooped;
    }
}
