using System;
using Framework_Module.Definitions.Behaviors.Movement;
using Framework_Module.GameData.Ai;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Behavior
{
    /// <summary>
    /// Follows the player's vertical movement, keeping horizontal
    /// position while adjusting up or down to chase.
    /// </summary>
    [Serializable]
    public class StalkerMovementAiBehavior : MovementBehavior<StalkerMovementAiBehaviorDefinition>
    {
        public override void Tick(float deltaTime, IVehicle vehicle)
        {
            var player = Services.Get<IPlayerController>()?.ControlledVehicle;

            if (player == null)
                return;

            float directionY = Mathf.Sign(player.Position.y - vehicle.Position.y);
            Vector2 targetVelocity = new Vector2(0, directionY * vehicle.Speed);
    
            vehicle.SetVelocity(Vector2.Lerp(vehicle.Velocity, targetVelocity, deltaTime * 5f));
        }


        public override void End(IVehicle vehicle)
        {
            base.End(vehicle);
        }

        public override void Start(IVehicle vehicle)
        {
            base.Start(vehicle);
            vehicle.SetVelocity(Vector2.zero); 
        }
        
        public override void Rebind(StalkerMovementAiBehaviorDefinition aiBehaviorDefinition)
        {
            Def = aiBehaviorDefinition;
        }

    }
}