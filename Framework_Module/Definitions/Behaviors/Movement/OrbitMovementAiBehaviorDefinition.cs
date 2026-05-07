using System;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions.Behaviors.Movement
{
    [Serializable]
    public class OrbitMovementAiBehaviorDefinition : MovementAiBehaviorDefinition
    {
        public override AiMovementType MovementType => AiMovementType.Orbit;
        [SerializeField] private float rotationRate = 1f;
        [SerializeField] private float arcRatioX = 1f;
        [SerializeField] private float arcRatioY = 1f;
        [SerializeField] private bool clockwise;
        [SerializeField]  private float startDegree;
        
        public float RotationRate => rotationRate;
        public float ArcRatioX => arcRatioX;
        public float ArcRatioY => arcRatioY;
        public bool Clockwise => clockwise;
        public float StartDegree => startDegree;
    }
}