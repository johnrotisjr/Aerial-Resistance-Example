using System;
using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Definitions.Behaviors;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Definitions.Behaviors.Movement;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.Gameplay;
using Framework_Module.GameData.Ai;
using Framework_Module.Interfaces;
using Framework_Module.Service;

namespace Ai_Module.Behaviors.Attack.Behavior
{
    /// <summary>
    /// Fires a single, highly accurate weapon aimed directly at
    /// the player's current position.
    /// </summary>
    [Serializable]
    public class SniperAttackAiBehavior : AttackBehavior<SniperAttackAiBehaviorDefinition>
    {
        protected override WeaponType WeaponType => WeaponType.Slug;
        
        public override void Rebind(SniperAttackAiBehaviorDefinition aiBehaviorDefinition)
        {
            Def = aiBehaviorDefinition;
        }
        public override void Tick(float deltaTime, IVehicle vehicle)
        {
            base.Tick(deltaTime, vehicle);
            
            var playerController = Services.Get<IPlayerController>();
            if (playerController?.ControlledVehicle == null)
            {
                return;
            }
            
            if(!CanFire)
                return;

            var socket = vehicle.GetSocketForType(WeaponType.Slug);
            var socketPosition = socket?.Position ?? vehicle.Position;
            var directionRaw = playerController.ControlledVehicle.Position - socketPosition;
            var fireGunEvent = new QueueSpawnWeaponEvent(WeaponType.Slug, socketPosition, directionRaw.normalized, vehicle.WorldRotation, AlignmentType.Foe);
            
            ResetFireCooldown();
            
            Services.Get<EventBus>().Publish(fireGunEvent);
        }

        public override void End(IVehicle vehicle)
        {
            base.End(vehicle);
        }

        public override void Start(IVehicle vehicle)
        {
            base.Start(vehicle);
        }
    }
}