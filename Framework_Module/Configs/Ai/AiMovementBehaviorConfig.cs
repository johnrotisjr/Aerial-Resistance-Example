using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Configs.Ai
{
    public abstract class AiMovementBehaviorConfig : ScriptableObject
    {
        public abstract AiMovementType MovementType { get; }
    }
}