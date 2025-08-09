using System;
using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;

namespace Ai_Module
{
    /// <summary>
    /// Moves an AI controlled vehicle into the gameplay area when spawned,
    /// either by traveling to a specific point or sliding onto the screen.
    /// </summary>
    public class AiSpawnPositioner
    {
        private readonly SpawnMovementInstruction spawnMovementInstruction;
        private readonly IViewportBoundsProvider viewportBoundsProvider;
        private readonly IVehicleController controller;
        private Vector2 enterVelocity;
        private readonly Action onComplete;
        
        private const float MoveToPositionTolerance = .25f;
        
        public AiSpawnPositioner(IViewportBoundsProvider viewportBoundsProvider, IVehicleController vehicleController, SpawnMovementInstruction spawnMovementInstruction, Action onCompleteCallback)
        {
            if (onCompleteCallback == null)
            {
                DebugLogger.Log("Callback is Null which is not allowed!", LogCategory.Ai, LogLevel.Error);
                return;
            }
                
            this.spawnMovementInstruction = spawnMovementInstruction;
            this.viewportBoundsProvider = viewportBoundsProvider;
            controller = vehicleController;
            onComplete = onCompleteCallback;
        }
        
        public void Update()
        {
            if (spawnMovementInstruction.IsUsingEndPosition)
                MoveToPosition();
            else
                MoveIntoView();
        }

        private void MoveToPosition()
        {
            var direction = (spawnMovementInstruction.EndPosition - controller.ControlledVehicle.Position);
            
            if (direction.sqrMagnitude < MoveToPositionTolerance)
            {
                controller.ControlledVehicle.SetVelocity(Vector2.zero);
                onComplete.Invoke();
                return;
            }
            
            controller.ControlledVehicle.SetVelocity(direction.normalized * controller.ControlledVehicle.Speed);
        }

        private void MoveIntoView()
        {
            if (controller.ControlledVehicle == null)
                return;

            if (enterVelocity == Vector2.zero)
                enterVelocity = GetVelocity(controller.ControlledVehicle);

            controller.ControlledVehicle.SetVelocity(enterVelocity);
            if (IsInView(controller.ControlledVehicle))
            {
                controller.ControlledVehicle.SetVelocity(Vector2.zero);
                onComplete.Invoke();
            }
        }

        private bool IsInView(IVehicle controlledVehicle)
        {
            if (controlledVehicle == null)
                return false;
            
            return viewportBoundsProvider.GetPixelAlignedViewport(-controlledVehicle.SpriteBounds.size/2).Contains(controlledVehicle.Position);
        }

        private Vector2 GetVelocity(IVehicle controlledVehicle)
        {
            var view = viewportBoundsProvider.GetPixelAlignedViewport();
            var pos = controlledVehicle.Position;
            var bounds = controlledVehicle.SpriteBounds;
            var halfSize = bounds.size * 0.5f;

            // Clamp position to viewport bounds with buffer for sprite size
            float clampedX = Mathf.Clamp(pos.x, view.xMin + halfSize.x, view.xMax - halfSize.x);
            float clampedY = Mathf.Clamp(pos.y, view.yMin + halfSize.y, view.yMax - halfSize.y);
            Vector2 clampedPoint = new Vector2(clampedX, clampedY);

            // Calculate normalized direction toward clamped point
            Vector2 direction = (clampedPoint - new Vector2(pos.x,pos.y)).normalized;

            return direction * controlledVehicle.Speed;
        }
    }
}