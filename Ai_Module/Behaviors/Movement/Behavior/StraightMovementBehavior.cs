using Debug_Module;
using Framework_Module.Configs.Ai;
using Framework_Module.Interfaces;
using Vector2 = UnityEngine.Vector2;

namespace Ai_Module.Behaviors.Movement.Behavior
{
    /// <summary>
    /// AI movement behavior that moves the Vehicle forward in a straight line.
    /// </summary>
    public class StraightMovementBehavior : IMovementBehavior
    {
        public int CompletedCycles => 0;
        
        public void Execute(IWorldObject worldObject)
        {
            if (worldObject is not IVehicle vehicle)
            {
                DebugLogger.Log("Controlled Vehicle not set.", LogCategory.Ai, LogLevel.Error);
                return;
            }
            
            var direction = vehicle.IsFacingRight ? Vector2.right : Vector2.left;
            vehicle.SetVelocity(direction * vehicle.Speed);
        }

        public void EndBehavior(IWorldObject worldObject)
        {
            worldObject.SetVelocity(Vector2.zero);
        }

        public void Reset(AiMovementBehaviorConfig data)
        {
            
        }
    }
}