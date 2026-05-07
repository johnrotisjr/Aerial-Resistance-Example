using System;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Definitions.Behaviors.Movement;
using UnityEngine;

namespace Framework_Module.Definitions
{
    [Serializable]
    public class IndependentMovementBehaviorDefinition : IndependentBehaviorDefinition
    {
        [SerializeReference] private MovementAiBehaviorDefinition movementBehaviorDefinition;
        public MovementAiBehaviorDefinition MovementBehaviorDefinition => movementBehaviorDefinition;
    }
}