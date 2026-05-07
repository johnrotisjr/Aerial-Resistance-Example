using System;
using Debug_Module;
using Framework_Module.Definitions.Behaviors.Movement;
using Framework_Module.GameData.Ai;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Behavior
{
    [Serializable]
    public class ArchingMovementAiBehavior : MovementBehavior<ArchingMovementAiBehaviorDefinition>
    {
        private float accumRadians;    
        private float startRadian = 0;
        private float endRadian = Mathf.PI;

        public override void Rebind(ArchingMovementAiBehaviorDefinition aiBehaviorDefinition)
        {
            Def = aiBehaviorDefinition;
        }
        
        public override void Tick(float deltaTime, IVehicle vehicle)
        {
            if (vehicle == null)
            {
                DebugLogger.Log("Null Vehicle!", LogCategory.Ai, LogLevel.Warning);
                return;
            }
            if (Def == null)
            {
                DebugLogger.Log("Null Definition!", LogCategory.Ai, LogLevel.Warning);
                vehicle.SetVelocity(Vector2.zero);
                return;
            }

            float arcSpan = Def.Clockwise
                ? Mathf.Repeat(startRadian - endRadian, 2f * Mathf.PI)
                : Mathf.Repeat(endRadian - startRadian, 2f * Mathf.PI);

            if (arcSpan <= 0f)
            {
                DebugLogger.Log("Arc Span is 0!", LogCategory.Ai, LogLevel.Warning);
                vehicle.SetVelocity(Vector2.zero);
                return;
            }

            // angular speed (radians/sec) = revolutions/sec * 2pi
            float radianDelta = Def.RevolutionsPerSecond * (2f * Mathf.PI) * deltaTime;
            accumRadians += Mathf.Abs(radianDelta);

            float angle = Def.Clockwise ? (startRadian - accumRadians) : (startRadian + accumRadians);

            // Tangent direction along an ellipse (0 rad is to the right)
            float tx = -Mathf.Sin(angle) * Def.ArcWidthMultiplier;
            float ty =  Mathf.Cos(angle) * Def.ArcHeightMultiplier;

            var direction = new Vector2(tx, ty).normalized;
            vehicle.SetVelocity(direction * vehicle.Speed);

            while (accumRadians >= arcSpan)
            {
                accumRadians -= arcSpan;
                RaiseCompletedMovementCycle();
            }
        }

        public override void End(IVehicle vehicle)
        {
            base.End(vehicle);
            vehicle?.SetVelocity(Vector2.zero);
        }

        public override void Start(IVehicle vehicle)
        {
            base.Start(vehicle);
            accumRadians = 0f;
            startRadian = Def.StartDegree * Mathf.Deg2Rad;
            endRadian = Def.EndDegree * Mathf.Deg2Rad;
        }
    }
}
