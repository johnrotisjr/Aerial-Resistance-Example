using Framework_Module.Configs.Ai;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Behavior
{
    /// <summary>
    /// Drives the vehicle directly toward the player's position in an
    /// aggressive suicide run.
    /// </summary>
    public class KamikazeMovementBehavior : IMovementBehavior
    {
        public int CompletedCycles => 0;
        
        private readonly IPlayerController playerController;
        
        public KamikazeMovementBehavior(IPlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void Execute(IWorldObject worldObject)
        {
            if (playerController.ControlledVehicle == null || worldObject is not IVehicle vehicle)
            {
                return;
            }

            var controlledVehicle = playerController.ControlledVehicle;
  
            var playerPosition = controlledVehicle.Position;
            var aiPosition = vehicle.Position;
            var directionToPlayer = playerPosition - aiPosition;
            if (directionToPlayer.x > -2)
            {
                return;
            }
            vehicle.SetVelocity(vehicle.Speed * Vector3.Normalize(directionToPlayer));
        }

        public void EndBehavior(IWorldObject worldObject)
        {
            
        }

        public void Reset(AiMovementBehaviorConfig data)
        {
            
        }
    }
}