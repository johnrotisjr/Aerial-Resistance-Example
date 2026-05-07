using System;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions.Behaviors.Movement
{
    [Serializable]
    public abstract class MovementAiBehaviorDefinition : AiBehaviorDefinition
    {
        public abstract AiMovementType MovementType { get; }
        [SerializeField]
        private float movementSpeedAdjustment = 0;
        [SerializeField]
        private bool isAdditiveMovementSpeedAdjustment = true;
        
        public float MovementSpeedAdjustment => movementSpeedAdjustment;
        public bool IsAdditiveAdjustment => isAdditiveMovementSpeedAdjustment;
    }
}