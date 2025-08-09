using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Configs.Ai
{
    public abstract class AiAttackBehaviorConfig : ScriptableObject
    {
        public abstract AiAttackType AttackType { get; }
    }
}