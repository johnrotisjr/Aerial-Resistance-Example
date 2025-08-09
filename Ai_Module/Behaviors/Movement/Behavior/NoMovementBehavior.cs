using Ai_Module.Behaviors.Movement.Data;
using Debug_Module;
using Framework_Module.Configs.Ai;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Behavior
{
    /// <summary>
    /// AI movement behavior that does not apply any movement.
    /// Serves as a passive or idle behavior for specific AI patterns.
    /// </summary>

    public class NoMovementBehavior : IMovementBehavior
    {
        public int CompletedCycles => 0;
 
        private Vector3 normalizedDriftDirection;
        
        public NoMovementBehavior()
        {
        }

        public void Execute(IWorldObject worldObject)
        {
            if (worldObject is not IVehicle vehicle)
            {
                DebugLogger.Log("Must be a Vehicle type.", LogCategory.Ai, LogLevel.Error);
                return;
            }
 
            vehicle.SetVelocity(normalizedDriftDirection);
        }

        public void EndBehavior(IWorldObject worldObject)
        {
            worldObject.SetVelocity(Vector2.zero);
        }

        public void Reset(AiMovementBehaviorConfig data)
        {
            if (data is NoMovementBehaviorConfig config)
            {
                normalizedDriftDirection = config.normalizedDriftDirection.normalized;
            }
            else
            {
                DebugLogger.Log("Incorrect data type for behavior", LogCategory.Ai, LogLevel.Error);
            }
        }
        
    }
}