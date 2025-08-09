using Debug_Module;
using Framework_Module.Configs.Ai;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.Gameplay;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;

namespace Ai_Module.Behaviors.Attack.Behavior
{
    /// <summary>
    /// Fires a single, highly accurate weapon aimed directly at
    /// the player's current position.
    /// </summary>

    public class SniperAttackBehavior : IAttackBehavior
    {
        private readonly IPlayerController playerController;
        private readonly EventBus eventBus;

        public SniperAttackBehavior(IPlayerController playerController, EventBus eventBus)
        {
            this.playerController = playerController;
            this.eventBus = eventBus;
        }

        public void Execute(IWorldObject worldObject)
        {
            if (playerController == null || worldObject is not IVehicle vehicle || playerController.ControlledVehicle == null)
            {
                return;
            }
            
            if(!vehicle.CanFireGun)
                return;

            var socket = vehicle.GetSocketForType(WeaponType.Slug);
            var socketPosition = socket?.Position ?? vehicle.Position;
            var directionRaw = playerController.ControlledVehicle.Position - socketPosition;
            var fireGunEvent = new SpawnWeaponEvent(WeaponType.Slug, socketPosition, directionRaw.normalized, vehicle.Rotation, AlignmentType.Foe);
            
            vehicle.ResetFireCooldown();
            
            eventBus.Publish(fireGunEvent);
        }

        public void EndBehavior(IWorldObject worldObject)
        {
            
        }

        public void Reset(AiAttackBehaviorConfig data)
        {
            
        }
    }
}