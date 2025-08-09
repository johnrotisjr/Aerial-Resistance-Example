using Framework_Module.Configs.Ai;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Behavior
{
    /// <summary>
    /// Follows the player's vertical movement, keeping horizontal
    /// position while adjusting up or down to chase.
    /// </summary>

    public class StalkerMovementBehavior : IMovementBehavior
    {
        public int CompletedCycles => 0;
        
        private readonly IPlayerController playerController;

        public StalkerMovementBehavior(IPlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void Execute(IWorldObject worldObject)
        {
            var player = playerController?.ControlledVehicle;

            if (player == null || worldObject is not IVehicle self)
                return;

            float directionY = Mathf.Sign(player.Position.y - self.Position.y);
            Vector2 targetVelocity = new Vector2(0, directionY * self.Speed);
    
            self.SetVelocity(Vector2.Lerp(self.Velocity, targetVelocity, Time.fixedDeltaTime * 5f));
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