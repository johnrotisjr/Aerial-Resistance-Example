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
 
    [Serializable]
    public class LaserSweepAttackAiBehavior : AttackBehavior<LaserSweepAttackAiBehaviorDefinition>
    {
        protected override WeaponType WeaponType => WeaponType.LaserSweep;
        
        private IWeapon laserInstance = null;
        private static float Normalize180(float z) => z > 180f ? z - 360f : z;

        public override void Rebind(LaserSweepAttackAiBehaviorDefinition aiBehaviorDefinition)
        {
            Def = aiBehaviorDefinition;
        }

        public override void Tick(float deltaTime, IVehicle vehicle)
        {
            base.Tick(deltaTime, vehicle);
        }

        public override void End(IVehicle vehicle)
        {
            base.End(vehicle);
            
            if(laserInstance != null)
                laserInstance.IsMarkedDestroyed = true;
        }

        public override void Start(IVehicle vehicle)
        {
            base.Start(vehicle);
            
            if(!CanFire)
                return;
            
            var eventBus = Services.Get<EventBus>();
            eventBus.Subscribe<WeaponSpawnedEvent>((e)=>OnWeaponSpawnedEvent(e, vehicle));
            var directionScalar = vehicle.IsFacingRight ? 1 : -1;
            var socket = vehicle.GetSocketForType(WeaponType.LaserSweep);
            var socketPosition = socket?.Position ?? vehicle.Position;
            var laserEvent = new QueueSpawnWeaponEvent(WeaponType.LaserSweep, socketPosition, new Vector2(0.5f * directionScalar, 0), vehicle.WorldRotation, AlignmentType.Foe, vehicle);
            eventBus.Publish(laserEvent);
            ResetFireCooldown();
        }

        private void OnWeaponSpawnedEvent(WeaponSpawnedEvent e, IVehicle vehicle)
        {
            if (e.ParentVehicle == vehicle && e.Weapon.WeaponType == WeaponType.LaserSweep)
            {
                laserInstance = e.Weapon;
            }
        }
    }
}