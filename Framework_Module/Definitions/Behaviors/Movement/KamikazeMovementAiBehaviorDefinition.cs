using System;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions.Behaviors.Movement
{
    [Serializable]
    public class KamikazeMovementAiBehaviorDefinition : MovementAiBehaviorDefinition
    {
        public override AiMovementType MovementType => AiMovementType.Kamikaze;
        public bool useContinuousAdjustment = true;
        public float abortTimeInSecs = -1;
        public Vector2 abortDirection = Vector2.zero;
    } 
}