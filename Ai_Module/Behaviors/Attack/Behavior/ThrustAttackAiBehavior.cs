using System;
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
 
    [Serializable]
    public class ThrustAttackAiBehavior : AttackBehavior<ThrustAttackAiBehaviorDefinition>
    {
        protected override WeaponType WeaponType => WeaponType.Thrust;
        public override void Rebind(ThrustAttackAiBehaviorDefinition aiBehaviorDefinition)
        {
            Def = aiBehaviorDefinition;
        }
        private IWeapon thrustInstance = null;
        public override void Tick(float deltaTime, IVehicle vehicle)
        {
            base.Tick(deltaTime, vehicle);
        }

        public override void End(IVehicle vehicle)
        {
            base.End(vehicle);
            if (thrustInstance == null)
                return;
            
            thrustInstance.IsMarkedDestroyed = true;
        }

        public override void Start(IVehicle vehicle)
        {
            base.Start(vehicle);
            
            if(!CanFire)
                return;
            
            var eventBus = Services.Get<EventBus>();
            eventBus.Subscribe<WeaponSpawnedEvent>((e)=>OnWeaponSpawnedEvent(e, vehicle));
            var socket = vehicle.GetSocketForType(WeaponType);
            var socketPosition = socket?.Position ?? vehicle.Position;
            var thrustEvent = new QueueSpawnWeaponEvent(WeaponType, socketPosition, Vector2.zero, vehicle.WorldRotation, AlignmentType.Foe, vehicle);
            eventBus.Publish(thrustEvent);
            ResetFireCooldown();
        }
        
        private void OnWeaponSpawnedEvent(WeaponSpawnedEvent e, IVehicle vehicle)
        {
            if (e.ParentVehicle == vehicle && e.Weapon.WeaponType == WeaponType.Thrust)
            {
                thrustInstance = e.Weapon;
            }
        }
    }
}