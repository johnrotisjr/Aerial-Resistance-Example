using System;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using Framework_Module.Interfaces;
using UnityEngine;

namespace World_Module.WorldObjects
{
    /// <summary>
    /// Represents a weapon fired by a vehicle.
    /// Tracks weapon type, damage, alignment, and collision behavior.
    /// </summary>

    internal class Weapon : WorldObject, IWeapon
    {
        public AlignmentType AlignmentType { get; set; }
        public float Damage => weaponDefinition?.Damage ?? 0;
        public float Speed => weaponDefinition?.Speed ?? 0;
        public WeaponType WeaponType => weaponDefinition?.Type ?? WeaponType.None;
        private bool DestroyOnCollision => weaponDefinition?.DestroyOnCollision ?? true;
        public bool IsMarkedDestroyed { get; set; } = false;
        
        private WeaponDefinition weaponDefinition;
        public WeaponDefinition WeaponDefinition => weaponDefinition;

        private IWeaponBehavior weaponMovementBehavior;
        private IWeaponBehaviorFactory factory;

        private bool isInitialized;

        public void HandleCollision()
        {
            IsMarkedDestroyed = DestroyOnCollision;
        }

        public void Initialize(IWeaponBehaviorFactory weaponBehaviorFactory)
        {
            if (!isInitialized)
            {
                factory = weaponBehaviorFactory;
                isInitialized = true;
            }
        }
        
        public void Set(WeaponDefinition definition)
        {
            weaponDefinition = definition;
            weaponMovementBehavior = factory.GetBehavior(weaponDefinition.Type);
            weaponMovementBehavior?.Start(this);
            IsMarkedDestroyed = false;
        }

        public void FixedUpdate()
        {
            weaponMovementBehavior?.Tick(Time.fixedDeltaTime,this);
        }

        private void OnDestroy()
        {
            Clear();
        }
        
        private void OnDisable()
        {
            Clear();
        }

        public void Clear()
        {
            weaponMovementBehavior?.End(this);
            weaponDefinition = null;
            IsMarkedDestroyed = false;
        }
    }
}
