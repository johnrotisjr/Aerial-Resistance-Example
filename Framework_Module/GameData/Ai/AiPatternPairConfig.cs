using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.GameData.Ai
{
    /// <summary>
    /// Represents a single behavior pattern in an AI routine, defining a movement and attack type.
    /// </summary>
    [CreateAssetMenu(fileName = "AiPatternPair", menuName = "Scriptable Objects/Ai/PatternPair")]
    public class AiPatternPairConfig : ScriptableObject
    {
        [SerializeField] public AiAttackType aiAttackType;
        [SerializeField] public AiMovementType aiMovementType;
    }
}
