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
    /// AI attack behavior that fires weapons straight ahead.
    /// </summary>

    public class StraightAttackBehavior : IAttackBehavior
    {
        private readonly EventBus eventBus;

        public StraightAttackBehavior(EventBus eventBus)
        {
            this.eventBus = eventBus;
        }
        
        public void Execute(IWorldObject worldObject)
        {
            if (worldObject is not IVehicle vehicle)
            {
                DebugLogger.Log("VehicleController is not the correct Type", LogCategory.Ai, LogLevel.Error);
                return;
            }
            
            if(!vehicle.CanFireGun)
                return;

            var directionScalar = vehicle.IsFacingRight ? 1 : -1;
            var socket = vehicle.GetSocketForType(WeaponType.Slug);
            var socketPosition = socket?.Position ?? vehicle.Position;
            var fireGunEvent = new SpawnWeaponEvent(WeaponType.Slug, socketPosition, Vector2.right * directionScalar, vehicle.Rotation, AlignmentType.Foe);
            
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