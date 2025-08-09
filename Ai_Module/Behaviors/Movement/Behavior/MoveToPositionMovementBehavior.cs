using Ai_Module.Behaviors.Movement.Data;
using Debug_Module;
using Framework_Module.Configs.Ai;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Behavior
{
    /// <summary>
    /// Moves the vehicle toward a designated world position and stops
    /// once the destination is reached.
    /// </summary>
    public class MoveToPositionMovementBehavior : IMovementBehavior
    {
        private bool hasCycled = false;
        public int CompletedCycles => hasCycled ? 1 : 0;
        private Vector3 endPosition;
        
        public MoveToPositionMovementBehavior()
        {
        }

        public void Execute(IWorldObject worldObject)
        {
            if (worldObject is not IVehicle vehicle)
            {
                DebugLogger.Log("Must be a Vehicle type.", LogCategory.Ai, LogLevel.Error);
                return;
            }
            
            var direction = endPosition - vehicle.Position;
            const float epsilonRoot = 0.1f;
            if (direction.sqrMagnitude < epsilonRoot * epsilonRoot)
            {
                vehicle.SetVelocity(Vector2.zero);
                hasCycled = true;
                return;
            }
            
            vehicle.SetVelocity(direction.normalized * vehicle.Speed);
        }

        public void EndBehavior(IWorldObject worldObject)
        {
            
        }

        public void Reset(AiMovementBehaviorConfig data)
        {
            if (data is MoveToPositionMovementBehaviorConfig config)
            {
                endPosition = config.endPosition;
            }
            else
            {
                DebugLogger.Log("Incorrect data type for behavior", LogCategory.Ai, LogLevel.Error);
            }
        }
    }
}