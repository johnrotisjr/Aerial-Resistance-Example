using System;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.Gameplay;
using Framework_Module.GameData.Ai;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;

namespace Ai_Module.Behaviors.Attack.Behavior
{
    /// <summary>
    /// Fires three weapons in a spread pattern, allowing the
    /// AI to cover a wider area in front of it.
    /// </summary>
    [Serializable]
    public class SprayAttackAiBehavior : AttackBehavior<SprayAttackAiBehaviorDefinition>
    {
        protected override WeaponType WeaponType => WeaponType.Slug;
        public override void Rebind(SprayAttackAiBehaviorDefinition aiBehaviorDefinition)
        {
            Def = aiBehaviorDefinition;
        }

        public override void Tick(float deltaTime, IVehicle vehicle)
        {
            base.Tick(deltaTime, vehicle);
            
            if(!CanFire)
                return;
            
            var directionScalar = vehicle.IsFacingRight ? 1 : -1;
            var socket = vehicle.GetSocketForType(WeaponType);
            var socketPosition = socket?.Position ?? vehicle.Position;

            var fireGunEvent1 = new QueueSpawnWeaponEvent(WeaponType, socketPosition, new Vector2(0.5f * directionScalar, Def.Angle).normalized, vehicle.WorldRotation, AlignmentType.Foe);
            var fireGunEvent2 = new QueueSpawnWeaponEvent(WeaponType, socketPosition, Vector2.right * directionScalar, vehicle.WorldRotation, AlignmentType.Foe);
            var fireGunEvent3 = new QueueSpawnWeaponEvent(WeaponType, socketPosition, new Vector2(0.5f * directionScalar, -Def.Angle).normalized, vehicle.WorldRotation, AlignmentType.Foe);
            
            ResetFireCooldown();

            var eventBus = Services.Get<EventBus>();
            eventBus.Publish(fireGunEvent1);
            eventBus.Publish(fireGunEvent2);
            eventBus.Publish(fireGunEvent3);
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