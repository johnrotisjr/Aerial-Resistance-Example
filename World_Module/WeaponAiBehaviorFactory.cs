using System;
using Ai_Module.Behaviors.Attack.Behavior;
using Framework_Module.Enums;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using World_Module.WeaponBehaviors;
using World_Module.WeaponMovementBehaviors;

namespace World_Module
{
    /// <summary>
    /// </summary>
    public class WeaponAiBehaviorFactory : IWeaponBehaviorFactory
    {
        private readonly IWorldStateManager worldStateManager;
        
        //TODO: Inject dependency
        public WeaponAiBehaviorFactory()
        {
            this.worldStateManager = Services.Get<IWorldStateManager>();
        }
        
        //TODO: use pooling
        public IWeaponBehavior GetBehavior(WeaponType type)
        {
            switch(type)
            {
                case WeaponType.Bullet: return new BulletWeaponMovementBehavior();
                case WeaponType.HomingMissile: return new HomingMissileWeaponMovementBehavior(worldStateManager);
                case WeaponType.Bomb: return new BombWeaponMovementBehavior();
                case WeaponType.Slug: return new SlugWeaponMovementBehavior();
                case WeaponType.LaserSweep: return new LaserSweepWeaponBehavior();
                case WeaponType.Thrust: return new ThrustWeaponMovementBehavior();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            };
        }
    }
}