using Ai_Module.Behaviors.Movement.Data;
using Debug_Module;
using Framework_Module.Configs.Ai;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Behavior
{
    /// <summary>
    /// Moves the vehicle forward while wobbling vertically using a sine wave
    /// pattern. Useful for simple enemy flight paths with slight variance.
    /// </summary>

    public class WobbleMovementBehavior : IMovementBehavior
    {
        private float time;
        private float rotationRate = 1f;
        private float amplitude = 1f; 
        
        public int CompletedCycles => Mathf.FloorToInt((time * rotationRate)/1);

        public WobbleMovementBehavior()
        {
            time = 0f;
        }

        public void Execute(IWorldObject worldObject)
        {
 
            if (worldObject is not IVehicle vehicle)
                return;

            time += Time.fixedDeltaTime;

            float verticalOffset = Mathf.Sin(time * rotationRate * 2 * Mathf.PI) * amplitude;
            Vector2 movement = new Vector2(-1f, verticalOffset).normalized;

            vehicle.SetVelocity(movement * vehicle.Speed);
        }

        public void EndBehavior(IWorldObject worldObject)
        {
            worldObject?.SetVelocity(Vector2.zero);
        }

        public void Reset(AiMovementBehaviorConfig data)
        {
            if (data is WobbleMovementBehaviorConfig wobbleData)
            {
                rotationRate = wobbleData.rotationRate;
                amplitude = wobbleData.amplitude;
            }
            else
            {
                DebugLogger.Log("Incorrect data type for behavior", LogCategory.Ai, LogLevel.Error);
            }
        }
    }
}