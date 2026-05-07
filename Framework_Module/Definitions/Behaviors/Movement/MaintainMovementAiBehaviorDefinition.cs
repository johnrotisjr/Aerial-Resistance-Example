using System;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions.Behaviors.Movement
{
    [Serializable]
    public class MaintainMovementAiBehaviorDefinition : MovementAiBehaviorDefinition
    {
        public override AiMovementType MovementType => AiMovementType.Maintain;
    }
}