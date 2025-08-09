using Debug_Module;
using Framework_Module.Configs.Ai;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Ai_Module.Behaviors.Movement.Behavior
{
    /// <summary>
    /// AI movement behavior that causes a Vehicle to bounce between the top and bottom of the screen.
    /// </summary>

    public class HoverMovementBehavior : IMovementBehavior
    {
        public int CompletedCycles { get; private set; }
        private Vector3? startPos;
        private int directionChangeCount = 0;
        
        private Coroutine coroutine;
        private bool isGoingSouth = false;
        readonly IViewportBoundsProvider viewportBoundsProvider;
        
        public HoverMovementBehavior(IViewportBoundsProvider viewportBoundsProvider)
        {
            this.viewportBoundsProvider = viewportBoundsProvider;
        }
        
        public void Execute(IWorldObject worldObject)
        {
            if (worldObject is not IVehicle vehicle)
            {
                DebugLogger.Log("Controlled Vehicle not set.", LogCategory.Ai, LogLevel.Error);
                return;
            }

            if (startPos == null)
                startPos = vehicle.Position;
            
            if (directionChangeCount == 2 && (startPos.Value - vehicle.Position).sqrMagnitude < float.Epsilon)
            {
                directionChangeCount = 0;
                CompletedCycles++;
            }
            
            var viewportRect = viewportBoundsProvider.GetPixelAlignedViewport(-vehicle.SpriteBounds.size*.5f);

            if (!isGoingSouth && vehicle.Position.y > viewportRect.max.y)
            {
                isGoingSouth = true;
                directionChangeCount++;
            }
            else if (isGoingSouth && vehicle.Position.y < viewportRect.min.y)
            {
                isGoingSouth = false;
                directionChangeCount++;
            }
            
            if (isGoingSouth)
            {
                vehicle.SetVelocity(Vector2.down * vehicle.Speed);
            }
            else
            {
                vehicle.SetVelocity(Vector2.up * vehicle.Speed);
            }
        }

        public void EndBehavior(IWorldObject worldObject)
        {
            worldObject.SetVelocity(Vector2.zero);
            startPos = null;
        }

        public void Reset(AiMovementBehaviorConfig data)
        {
            
        }
    }
}