using UnityEngine;
using UnityEngine.Serialization;

namespace Framework_Module.Configs.Ai
{
    /// <summary>
    /// Represents a single behavior pattern in an AI routine, defining a movement and attack type.
    /// </summary>
    [CreateAssetMenu(fileName = "AiPatternPair", menuName = "Scriptable Objects/Ai/PatternPair")]
    public class AiPatternPairConfig : ScriptableObject
    {
        [FormerlySerializedAs("aiAttackBehavior")] [SerializeField] public AiAttackBehaviorConfig aiAttackBehaviorConfig;
        [FormerlySerializedAs("aiMovementBehavior")] [SerializeField] public AiMovementBehaviorConfig aiMovementBehaviorConfig;
    }
}
