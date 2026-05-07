using System;
using System.Collections;
using Debug_Module;
using Framework_Module.Core;
using Framework_Module.GameData.Instructions;
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
        private readonly Action onComplete;
        private Coroutine routine;
        private bool isRunning = false;
        
        public bool IsRunning => isRunning;
        
        private const float MoveToPositionTolerance = .25f;
        
        public AiSpawnPositioner(IViewportBoundsProvider viewportBoundsProvider, IVehicleController vehicleController, SpawnMovementInstruction spawnMovementInstruction, Action onCompleteCallback)
        {
            if (onCompleteCallback == null)
            {
                DebugLogger.Log("Callback is Null which is not allowed!", LogCategory.Ai, LogLevel.Error);
                throw new ArgumentNullException(nameof(onCompleteCallback));
            }
                
            this.spawnMovementInstruction = spawnMovementInstruction;
            this.viewportBoundsProvider = viewportBoundsProvider;
            controller = vehicleController;
            onComplete = onCompleteCallback;
        }

        public void Begin()
        {
            Reset();
            if (controller.ControlledVehicle == null)
            {
                DebugLogger.Log("No ControlledVehicle assigned.", LogCategory.Ai, LogLevel.Warning);
                return;
            }
            isRunning = true;
            routine = spawnMovementInstruction.IsEnabled ? 
                CoroutineRunner.Begin(MoveToPosition()) : 
                CoroutineRunner.Begin(MoveIntoView());
        }

        public void End()
        {
            Reset();
        }

        private void Reset()
        {
            if (routine != null)
            {
                CoroutineRunner.End(routine);
                routine = null;
            }
            isRunning = false;
        }

        private IEnumerator MoveToPosition()
        {
            var vehicle = controller.ControlledVehicle;
            if (vehicle == null)
            {
                isRunning = false;
                yield break;
            }
            var bounds = viewportBoundsProvider.WorldViewBounds;

            var actualPosX = bounds.min.x + spawnMovementInstruction.ViewportXPercent * bounds.size.x;
            var actualPosY = bounds.min.y + spawnMovementInstruction.ViewportYPercent * bounds.size.y;
            
            Vector3 target = new Vector2(actualPosX, actualPosY);
            float toleranceSquared = MoveToPositionTolerance * MoveToPositionTolerance;

            while (true)
            {
                vehicle = controller.ControlledVehicle;
                if (vehicle == null)
                {
                    isRunning = false;
                    yield break;
                }

                var delta = target - vehicle.Position;
                if (delta.sqrMagnitude <= toleranceSquared)
                    break;

                var vel = delta.normalized * vehicle.Speed;
                vehicle.SetVelocity(vel);
                yield return null;
            }
            
            vehicle.SetVelocity(Vector2.zero);
            isRunning = false;
            onComplete.Invoke();
        }

        private IEnumerator MoveIntoView()
        {
            var vehicle = controller.ControlledVehicle;
            if (vehicle == null)
            {
                isRunning = false;
                yield break;
            }

            Vector2 enterVelocity = GetEnterVelocity(vehicle);
            vehicle.SetVelocity(enterVelocity);

            while (true)
            {
                vehicle = controller.ControlledVehicle;
                if (vehicle == null)
                {
                    isRunning = false;
                    yield break;
                }

                if (IsInView(vehicle))
                    break;

                vehicle.SetVelocity(GetEnterVelocity(vehicle));
                yield return null;
            }

            vehicle.SetVelocity(Vector2.zero);
            isRunning = false;
            onComplete.Invoke();
        }
        
        private bool IsInView(IVehicle vehicle)
        {
            // Treat the vehicle as inside view when its sprite rect overlaps the viewport rect
            Vector3 half = vehicle.SpriteBounds.size * 0.5f;
            Rect viewport = viewportBoundsProvider.GetPixelAlignedViewport(-half);
            
            Rect spriteRect = new Rect(vehicle.Position, vehicle.SpriteBounds.size);
            return viewport.Overlaps(spriteRect);
        }

        private Vector2 GetEnterVelocity(IVehicle vehicle)
        {
            Rect viewport = viewportBoundsProvider.GetPixelAlignedViewport();
            Vector2 pos = vehicle.Position;

            // Nearest clamped point inside the battlefield
            float cx = Mathf.Clamp(pos.x, viewport.xMin, viewport.xMax);
            float cy = Mathf.Clamp(pos.y, viewport.yMin, viewport.yMax);
            Vector2 clamped = new Vector2(cx, cy);

            Vector2 dir = (clamped - pos).normalized;
            if (dir.sqrMagnitude <= float.Epsilon)
            {
                // Already inside or exactly on the edge, nudge inward along x
                return vehicle.Velocity;
            }

            return dir * vehicle.Speed;
        }
    }
}