using System;
using Ai_Module.Behaviors.Movement.Data;
using Debug_Module;
using Framework_Module.Configs.Ai;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Behavior
{
    /// <summary>
    /// Moves the Vehicle in a continuous forward path while oscillating vertically in a sine wave,
    /// creating an arching or wave-like motion.
    /// </summary>
    public class OrbitMovementBehavior : IMovementBehavior
    {
        public int CompletedCycles => Mathf.FloorToInt((time * rotationRate)/1);
        
        private float time;
        private float rotationRate = 1f; 
        private float arcRatioX = 1f;  
        private float arcRatioY = 1f;
        private bool clockwise = false;
        private float startDegree = 0;

        public OrbitMovementBehavior()
        {
            time = 0f;
        }

        public void Execute(IWorldObject worldObject)
        {
            if (worldObject is not IVehicle vehicle)
                return;

            time += Time.fixedDeltaTime;
            var offset = vehicle.IsFacingRight ? 0 : Mathf.PI / 2;
            var angle = offset + time * rotationRate * 2 * Mathf.PI + startDegree * Mathf.Deg2Rad;

            if (clockwise)
                angle = -angle;
            
            var x = Mathf.Cos(angle) * arcRatioX;
            var y = Mathf.Sin(angle) * arcRatioY;
            
            var direction = new Vector2(x, y).normalized;

            vehicle.SetVelocity(direction * vehicle.Speed);
        }

        public void EndBehavior(IWorldObject worldObject)
        {
            worldObject?.SetVelocity(Vector2.zero);
        }

        public void Reset(AiMovementBehaviorConfig data)
        {
            if (data is OrbitMovementBehaviorConfig config)
            {
                rotationRate = config.rotationRate;
                arcRatioX = config.arcRatioX;
                arcRatioY = config.arcRatioY;
                clockwise = config.clockwise;
                startDegree = config.startDegree;
            }
            else
            {
                DebugLogger.Log("Incorrect data type for behavior", LogCategory.Ai, LogLevel.Error);
            }
        }
    }
}