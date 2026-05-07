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
    /// Launches weapons in all cardinal and diagonal directions
    /// forming a radial burst around the firing vehicle.
    /// </summary>
    [Serializable]
    public class RadialAttackAiBehavior : AttackBehavior<RadialAttackAiBehaviorDefinition>
    {
        protected override WeaponType WeaponType => WeaponType.Slug;
        private Vector3 projExtents;
        private float spacing;
 
        public override void Tick(float deltaTime, IVehicle vehicle)
        {
            base.Tick(deltaTime, vehicle);
            
            if(!CanFire)
                return;

            ResetFireCooldown();
            
            //Right
            var fireGunEvent1 = new QueueSpawnWeaponEvent(WeaponType, vehicle.Position, new Vector2(0.866025f, 0.5f).normalized, vehicle.WorldRotation, AlignmentType.Foe);
            var fireGunEvent2 = new QueueSpawnWeaponEvent(WeaponType, vehicle.Position, Vector2.right, vehicle.WorldRotation, AlignmentType.Foe);
            var fireGunEvent3 = new QueueSpawnWeaponEvent(WeaponType, vehicle.Position, new Vector2(0.866025f, -0.5f).normalized, vehicle.WorldRotation, AlignmentType.Foe);
            
            //Left
            var fireGunEvent4 = new QueueSpawnWeaponEvent(WeaponType, vehicle.Position, new Vector2(-0.866025f, 0.5f).normalized, vehicle.WorldRotation, AlignmentType.Foe);
            var fireGunEvent5 = new QueueSpawnWeaponEvent(WeaponType, vehicle.Position, Vector2.left, vehicle.WorldRotation, AlignmentType.Foe);
            var fireGunEvent6 = new QueueSpawnWeaponEvent(WeaponType, vehicle.Position, new Vector2(-0.866025f, -0.5f).normalized, vehicle.WorldRotation, AlignmentType.Foe);
            
            //Top
            var fireGunEvent7 = new QueueSpawnWeaponEvent(WeaponType, vehicle.Position, new Vector2(0.5f, 0.866025f).normalized, vehicle.WorldRotation, AlignmentType.Foe);
            var fireGunEvent8 = new QueueSpawnWeaponEvent(WeaponType, vehicle.Position, Vector2.up, vehicle.WorldRotation, AlignmentType.Foe);
            var fireGunEvent9 = new QueueSpawnWeaponEvent(WeaponType, vehicle.Position, new Vector2(-0.5f, 0.866025f).normalized, vehicle.WorldRotation, AlignmentType.Foe);
            
            //Bottom
            var fireGunEvent10 = new QueueSpawnWeaponEvent(WeaponType, vehicle.Position, new Vector2(0.5f, -0.866025f).normalized, vehicle.WorldRotation, AlignmentType.Foe);
            var fireGunEvent11 = new QueueSpawnWeaponEvent(WeaponType, vehicle.Position, Vector2.down, vehicle.WorldRotation, AlignmentType.Foe);
            var fireGunEvent12 = new QueueSpawnWeaponEvent(WeaponType, vehicle.Position, new Vector2(-0.5f, -0.866025f).normalized, vehicle.WorldRotation, AlignmentType.Foe);

            var eventBus = Services.Get<EventBus>();
            eventBus.Publish(fireGunEvent1);
            eventBus.Publish(fireGunEvent2);
            eventBus.Publish(fireGunEvent3);
            eventBus.Publish(fireGunEvent4);
            eventBus.Publish(fireGunEvent5);
            eventBus.Publish(fireGunEvent6);
            eventBus.Publish(fireGunEvent7);
            eventBus.Publish(fireGunEvent8);
            eventBus.Publish(fireGunEvent9);
            eventBus.Publish(fireGunEvent10);
            eventBus.Publish(fireGunEvent11);
            eventBus.Publish(fireGunEvent12);
        }

        public override void End(IVehicle vehicle)
        {
            base.End(vehicle);
        }
        
        public override void Rebind(RadialAttackAiBehaviorDefinition aiBehaviorDefinition)
        {
            Def = aiBehaviorDefinition;
        }

        public override void Start(IVehicle vehicle)
        {
            base.Start(vehicle);
            if (!Services.Get<IConfigDatabase>().GetWeaponDefinition(WeaponType, out var weaponData))
            {
                DebugLogger.Log($"Could not find data for WeaponType {WeaponType}", LogCategory.Ai, LogLevel.Error);
                return;
            } 
            projExtents = weaponData.Prefab.GetComponent<SpriteRenderer>().bounds.extents;
            spacing = projExtents.x*2.5f;
        }
    }
}