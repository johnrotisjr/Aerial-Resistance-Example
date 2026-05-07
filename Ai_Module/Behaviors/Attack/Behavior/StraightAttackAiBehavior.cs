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
using UnityEngine;

namespace Ai_Module.Behaviors.Attack.Behavior
{
    /// <summary>
    /// AI attack behavior that fires weapons straight ahead.
    /// </summary>
    [Serializable]
    public class StraightAttackAiBehavior : AttackBehavior<StraightAttackAiBehaviorDefinition>
    {
        protected override WeaponType WeaponType => WeaponType.Slug;
        public override void Rebind(StraightAttackAiBehaviorDefinition aiBehaviorDefinition)
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
            var fireGunEvent = new QueueSpawnWeaponEvent(WeaponType, socketPosition, Vector2.right * directionScalar, vehicle.WorldRotation, AlignmentType.Foe);
            
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