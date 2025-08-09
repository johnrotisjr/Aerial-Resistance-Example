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
    /// Launches weapons in all cardinal and diagonal directions
    /// forming a radial burst around the firing vehicle.
    /// </summary>

    public class RadialAttackBehavior : IAttackBehavior
    {
        private readonly EventBus eventBus;
        private readonly IConfigDatabase configDatabase;
        
        private const WeaponType WeaponType = Framework_Module.Enums.WeaponType.Slug;
        private Vector3 projExtents;
        private float spacing;
        
        public RadialAttackBehavior(IConfigDatabase configDatabase, EventBus eventBus)
        {
            this.eventBus = eventBus;
            this.configDatabase = configDatabase;
        }
 
        public void Execute(IWorldObject worldObject)
        {
            if (worldObject is not IVehicle vehicle)
            {
                DebugLogger.Log("Vehicle is not the correct Type", LogCategory.Ai, LogLevel.Error);
                return;
            }
            
            if(!vehicle.CanFireGun)
                return;

            var position = vehicle.Position;
            
            //Right
            var rightOffset = position.x + vehicle.SpriteBounds.extents.x + projExtents.x;
            var pos1 = new Vector3(rightOffset, position.y + spacing, position.z);
            var pos2 = new Vector3(rightOffset, position.y, position.z);
            var pos3 = new Vector3(rightOffset, position.y - spacing, position.z);
            var fireGunEvent1 = new SpawnWeaponEvent(WeaponType, pos1, new Vector2(0.866025f, 0.5f).normalized, vehicle.Rotation, AlignmentType.Foe);
            var fireGunEvent2 = new SpawnWeaponEvent(WeaponType, pos2, Vector2.right, vehicle.Rotation, AlignmentType.Foe);
            var fireGunEvent3 = new SpawnWeaponEvent(WeaponType, pos3, new Vector2(0.866025f, -0.5f).normalized, vehicle.Rotation, AlignmentType.Foe);
            
            //Left
            var leftOffset = position.x - vehicle.SpriteBounds.extents.x - projExtents.x;
            var pos4 = new Vector3(leftOffset, position.y + spacing, position.z);
            var pos5 = new Vector3(leftOffset, position.y, position.z);
            var pos6 = new Vector3(leftOffset, position.y - spacing, position.z);
            var fireGunEvent4 = new SpawnWeaponEvent(WeaponType, pos4, new Vector2(-0.866025f, 0.5f).normalized, vehicle.Rotation, AlignmentType.Foe);
            var fireGunEvent5 = new SpawnWeaponEvent(WeaponType, pos5, Vector2.left, vehicle.Rotation, AlignmentType.Foe);
            var fireGunEvent6 = new SpawnWeaponEvent(WeaponType, pos6, new Vector2(-0.866025f, -0.5f).normalized, vehicle.Rotation, AlignmentType.Foe);
            
            //Top
            var topOffset = position.y + vehicle.SpriteBounds.extents.y + projExtents.y;
            var pos7 = new Vector3(position.x + spacing, topOffset, position.z);
            var pos8 = new Vector3(position.x, topOffset, position.z);
            var pos9 = new Vector3(position.x - spacing, topOffset, position.z);
            var fireGunEvent7 = new SpawnWeaponEvent(WeaponType, pos7, new Vector2(0.5f, 0.866025f).normalized, vehicle.Rotation, AlignmentType.Foe);
            var fireGunEvent8 = new SpawnWeaponEvent(WeaponType, pos8, Vector2.up, vehicle.Rotation, AlignmentType.Foe);
            var fireGunEvent9 = new SpawnWeaponEvent(WeaponType, pos9, new Vector2(-0.5f, 0.866025f).normalized, vehicle.Rotation, AlignmentType.Foe);
            
            //Bottom
            var bottomOffset = position.y - vehicle.SpriteBounds.extents.y - projExtents.y;
            var pos10 = new Vector3(position.x + spacing, bottomOffset, position.z);
            var pos11 = new Vector3(position.x, bottomOffset, position.z);
            var pos12 = new Vector3(position.x - spacing, bottomOffset, position.z);
            var fireGunEvent10 = new SpawnWeaponEvent(WeaponType, pos10, new Vector2(0.5f, -0.866025f).normalized, vehicle.Rotation, AlignmentType.Foe);
            var fireGunEvent11 = new SpawnWeaponEvent(WeaponType, pos11, Vector2.down, vehicle.Rotation, AlignmentType.Foe);
            var fireGunEvent12 = new SpawnWeaponEvent(WeaponType, pos12, new Vector2(-0.5f, -0.866025f).normalized, vehicle.Rotation, AlignmentType.Foe);
            
            vehicle.ResetFireCooldown();
            
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

        public void EndBehavior(IWorldObject worldObject)
        {
            
        }
        
        public void Reset(AiAttackBehaviorConfig data)
        {
            if (!configDatabase.GetWeaponDefinition(WeaponType, out var weaponData))
            {
                DebugLogger.Log($"Could not find data for WeaponType {WeaponType}", LogCategory.Ai, LogLevel.Error);
                return;
            }
            projExtents = weaponData.Prefab.GetComponent<SpriteRenderer>().bounds.extents;
            spacing = projExtents.x*2.5f;
        }
    }
}