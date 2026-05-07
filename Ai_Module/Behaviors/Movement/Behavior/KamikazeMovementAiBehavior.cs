using System;
using Framework_Module.Definitions;
using Framework_Module.Definitions.Behaviors;
using Framework_Module.Definitions.Behaviors.Movement;
using Framework_Module.GameData.Ai;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Behavior
{
    /// <summary>
    /// Drives the vehicle directly toward the player's position in an
    /// aggressive suicide run.
    /// </summary>
    [Serializable]
    public class KamikazeMovementAiBehavior : MovementBehavior<KamikazeMovementAiBehaviorDefinition>
    {
        private float timeSinceStart = 0;
        private bool setVelocity = false;
        
        public override void Tick(float deltaTime, IVehicle vehicle)
        {
            timeSinceStart += deltaTime;
            var playerController = Services.Get<IPlayerController>();
            if (Def == null || playerController?.ControlledVehicle == null)
            {
                return;
            }

            var controlledVehicle = playerController.ControlledVehicle;
            var playerPosition = controlledVehicle.Position;
            var aiPosition = vehicle.Position;
            var directionToPlayer = playerPosition - aiPosition;
            if (Def.abortTimeInSecs > 0 && timeSinceStart >= Def.abortTimeInSecs)
            {
                Vector3 dir = Def.abortDirection == Vector2.zero ? directionToPlayer.normalized : Def.abortDirection;
                vehicle.SetVelocity(vehicle.Speed * dir);
                return;
            }

            if (!setVelocity || Def.useContinuousAdjustment)
            {
                vehicle.SetVelocity(vehicle.Speed * Vector3.Normalize(directionToPlayer));
                setVelocity = true;
            }
        }
        
        public override void End(IVehicle vehicle)
        {
            base.End(vehicle);
        }
        
        public override void Rebind(KamikazeMovementAiBehaviorDefinition aiBehaviorDefinition)
        {
            Def = aiBehaviorDefinition;
        }
        
        public override void Start(IVehicle vehicle)
        {
            base.Start(vehicle);
            timeSinceStart = 0;
            setVelocity = false;
        }

    }
}