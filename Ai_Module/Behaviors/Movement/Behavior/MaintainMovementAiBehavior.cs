using System;
using Framework_Module.Definitions.Behaviors.Movement;
using Framework_Module.GameData.Ai;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Behavior
{
    [Serializable]
    public class MaintainMovementAiBehavior : MovementBehavior<MaintainMovementAiBehaviorDefinition>
    {
        public override void Tick(float deltaTime, IVehicle vehicle)
        {
        }

        public override void End(IVehicle vehicle)
        {
            base.End(vehicle);
        }

        public override void Start(IVehicle vehicle)
        {
            base.Start(vehicle);
            vehicle.SetVelocity(Vector2.zero);
        }
    
        public override void Rebind(MaintainMovementAiBehaviorDefinition aiBehaviorDefinition)
        {
            Def = aiBehaviorDefinition;
        }

    }
}
