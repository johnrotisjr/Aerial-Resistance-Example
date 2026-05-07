using System;
using Framework_Module.Definitions.Behaviors.Movement;
using Framework_Module.GameData.Ai;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Behavior
{
    [Serializable]
    public class OrbitMovementAiBehavior : MovementBehavior<OrbitMovementAiBehaviorDefinition>
    {
        private float phase;
    
        public override void Tick(float deltaTime, IVehicle vehicle)
        {
            float prev = phase;
            float deltaCycles = Def.RotationRate * deltaTime;
            phase = Mathf.Repeat(phase + deltaCycles, 1f);

            // angle for this frame
            float offset = vehicle.IsFacingRight ? 0f : Mathf.PI / 2f;
            float angle = offset + (Def.Clockwise ? -1f : 1f) * (phase * 2f * Mathf.PI) + Def.StartDegree * Mathf.Deg2Rad;

            float x = Mathf.Cos(angle) * Def.ArcRatioX;
            float y = Mathf.Sin(angle) * Def.ArcRatioY;
            var direction = new Vector2(x, y).normalized;
            vehicle.SetVelocity(direction * vehicle.Speed);

            // detect wrap: if phase wrapped from near 1 back to 0, a cycle completed
            if (phase < prev)
                RaiseCompletedMovementCycle();
        }

        public override void End(IVehicle vehicle)
        {
            base.End(vehicle);
            vehicle?.SetVelocity(Vector2.zero);
        }

        public override void Start(IVehicle vehicle)
        {
            base.Start(vehicle);
            phase = 0f;
        }
    
        public override void Rebind(OrbitMovementAiBehaviorDefinition aiBehaviorDefinition)
        {
            Def = aiBehaviorDefinition;
        }

    }
}