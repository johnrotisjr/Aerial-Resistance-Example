using System;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions.Behaviors.Movement
{

    [Serializable]
    public class TeleportMovementAiBehaviorDefinition : MovementAiBehaviorDefinition
    {
        public override AiMovementType MovementType => AiMovementType.Teleport;
        [SerializeField] private Vector3 teleportPosition;
        public Vector3 TeleportPosition => teleportPosition;
    }
}