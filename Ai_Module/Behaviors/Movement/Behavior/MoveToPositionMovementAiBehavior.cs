using System;
using Framework_Module.Definitions.Behaviors.Movement;
using Framework_Module.GameData.Ai;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Behavior
{
    /// <summary>
    /// Moves the vehicle toward a designated world position and stops
    /// once the destination is reached.
    /// </summary>
    ///
    [Serializable]
    public class MoveToPositionMovementAiBehavior : MovementBehavior<MoveToPositionMovementAiBehaviorDefinition>
    {
        private bool hasCycled = false;

        public override void Tick(float deltaTime, IVehicle vehicle)
        {
            var direction = Def.EndPosition - vehicle.Position;
            float dist = direction.magnitude;
            float maxStep = vehicle.Speed * deltaTime;
 
            if (dist <= maxStep)
            {
                vehicle.SetVelocity(Vector2.zero);
                if(!hasCycled)
                    RaiseCompletedMovementCycle();
                hasCycled = true;
                return;
            }
            
            vehicle.SetVelocity(direction.normalized * vehicle.Speed);
        }

        public override void End(IVehicle vehicle)
        {
            base.End(vehicle);
            vehicle.SetVelocity(Vector2.zero);
        }
        
        public override void Rebind(MoveToPositionMovementAiBehaviorDefinition aiBehaviorDefinition)
        {
            Def = aiBehaviorDefinition;
        }

        public override void Start(IVehicle vehicle)
        {
            base.Start(vehicle);
            hasCycled = false;
        }

    }
}