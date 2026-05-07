using System;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions.Behaviors.Attack
{
    [Serializable]
    public sealed class SprayAttackAiBehaviorDefinition : AttackAiBehaviorDefinition
    {
        public override AiAttackType AttackType => AiAttackType.Spray;
        [SerializeField] private float angle = 0.5f;
        public float Angle => angle;
    }
}