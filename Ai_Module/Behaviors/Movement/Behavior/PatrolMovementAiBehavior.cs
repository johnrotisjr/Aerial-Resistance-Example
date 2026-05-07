using System;
using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Definitions.Behaviors;
using Framework_Module.Definitions.Behaviors.Movement;
using Framework_Module.GameData.Ai;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Ai_Module.Behaviors.Movement.Behavior
{
    /// <summary>
    /// AI movement behavior that causes a Vehicle to bounce between the top and bottom of the screen.
    /// </summary>
    [Serializable]
    public class PatrolMovementAiBehavior : MovementBehavior<PatrolMovementAiBehaviorDefinition>
    {
        private Vector3? startPos;
        private int directionChangeCount = 0;
        private Coroutine coroutine;
        private bool isGoingSouth = false;
        
        public override void Rebind(PatrolMovementAiBehaviorDefinition aiBehaviorDefinition)
        {
            Def = aiBehaviorDefinition;
        }
        
        public override void Tick(float deltaTime, IVehicle vehicle)
        {
            if (startPos == null)
                startPos = vehicle.Position;
            
            if (directionChangeCount >= 2 && startPos.Value.y - vehicle.Position.y< float.Epsilon)
            {
                directionChangeCount = 0;
                RaiseCompletedMovementCycle();
            }
            
            var viewportRect = Services.Get<IViewportBoundsProvider>().GetPixelAlignedViewport(-vehicle.SpriteBounds.size*.5f, true);

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

        public override void End(IVehicle vehicle)
        {
            base.End(vehicle);
        }

        public override void Start(IVehicle vehicle)
        {
            base.Start(vehicle);
            startPos = null;
            vehicle.SetVelocity(Vector2.zero);
        }

    }
}