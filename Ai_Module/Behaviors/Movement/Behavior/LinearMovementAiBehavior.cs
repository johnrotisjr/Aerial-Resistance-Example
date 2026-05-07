using System;
using Framework_Module.Definitions;
using Framework_Module.Definitions.Behaviors;
using Framework_Module.Definitions.Behaviors.Movement;
using Framework_Module.GameData.Ai;
using Framework_Module.Interfaces;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Ai_Module.Behaviors.Movement.Behavior
{
    /// <summary>
    /// AI movement behavior that moves the Vehicle forward in a straight line.
    /// </summary>
    [Serializable]
    public class LinearMovementAiBehavior : MovementBehavior<LinearMovementAiBehaviorDefinition>
    {
        public override void Rebind(LinearMovementAiBehaviorDefinition aiBehaviorDefinition)
        {
            Def = aiBehaviorDefinition;
        }

        public override void Tick(float deltaTime, IVehicle vehicle)
        {
            float rad = Def.Angle * Mathf.Deg2Rad;
            var direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
            var velocity = direction * vehicle.Speed;
            vehicle.SetVelocity(velocity);
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
    }
}