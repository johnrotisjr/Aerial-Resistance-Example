using System;
using Framework_Module.Definitions.Behaviors.Movement;
using Framework_Module.GameData.Ai;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Behavior
{
    /// <summary>
    /// Moves the vehicle forward while wobbling vertically using a sine wave
    /// pattern. Useful for simple enemy flight paths with slight variance.
    /// </summary>

    [Serializable]
    public class WobbleMovementAiBehavior : MovementBehavior<WobbleMovementAiBehaviorDefinition>
    {
        private float time;
        private float baseY;
        private float previousPhase = 0f;

        public override void Tick(float deltaTime, IVehicle vehicle)
        {
            if (vehicle == null || Def == null)
                return;

            time += deltaTime;

            // 0-2Pi is 1 phase
            float phase = (time * Def.Frequency) % 1f;

            //sin value before shifting based or limits
            float raw = Mathf.Sin(phase * 2f * Mathf.PI);

            float y = 0;
            
            // raw absolute will give the time for the lerp
            float t = Mathf.Abs(raw);
            
            //Find the amplitude limit
            float limit = raw >= 0f ? Def.PositiveLimitPercent : Def.NegativeLimitPercent;

            if (limit > 0f)
            {
                float shaped = Def.FlattenAtLimit
                    ? Mathf.Sign(raw) * Mathf.Min(t, limit)
                    : Mathf.Sign(raw) * Mathf.Lerp(0f, limit, t);

                y = baseY + shaped * Def.Amplitude;
            }
            else
            {
                y = baseY + raw * Def.Amplitude;
            }

            // Constant forward motion based on facing
            float xVel = (vehicle.IsFacingRight ? 1f : -1f) * vehicle.Speed;
            
            //velocity only moves forward
            vehicle.SetVelocity(new Vector2(xVel, 0f));

            // set Vertical position
            vehicle.Position = new Vector3(vehicle.Position.x, y, vehicle.Position.z);

            // Cycle complete event when phase wraps
            if (phase < previousPhase)
            {
                RaiseCompletedMovementCycle();
            }

            previousPhase = phase;
        }

        public override void End(IVehicle vehicle)
        {

            base.End(vehicle);
        }

        public override void Start(IVehicle vehicle)
        {
            base.Start(vehicle);
            time = 0f;
            previousPhase = 0f;
            if (vehicle == null)
                return;
            
            vehicle.SetVelocity(Vector2.zero);
            baseY = vehicle.Position.y;
        }
        
        public override void Rebind(WobbleMovementAiBehaviorDefinition aiBehaviorDefinition)
        {
            Def = aiBehaviorDefinition;
        }

    }
}