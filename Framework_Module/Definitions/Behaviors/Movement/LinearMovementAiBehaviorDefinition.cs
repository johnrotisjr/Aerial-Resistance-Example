using System;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions.Behaviors.Movement
{
    [Serializable]
    public class LinearMovementAiBehaviorDefinition : MovementAiBehaviorDefinition
    {
        public override AiMovementType MovementType => AiMovementType.Linear;
        [SerializeField] private float angle = 0f;
        public float Angle => angle;
    }
}