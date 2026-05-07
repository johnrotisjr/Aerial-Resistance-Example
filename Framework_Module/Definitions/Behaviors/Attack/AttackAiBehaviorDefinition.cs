using System;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions.Behaviors.Attack
{

    [Serializable]
    public abstract class AttackAiBehaviorDefinition : AiBehaviorDefinition
    {
        public abstract AiAttackType AttackType { get; }
        [SerializeField]
        private float attackRateAdjustment = 0;
        [SerializeField]
        private bool isAdditiveAttackRateAdjustment = true;
        
        public float AttackRateAdjustment => attackRateAdjustment;
        public bool IsAdditiveAdjustment => isAdditiveAttackRateAdjustment;
    }
}