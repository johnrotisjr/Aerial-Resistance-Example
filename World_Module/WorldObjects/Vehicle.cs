using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.Combat;
using Framework_Module.GameData.Data;
using Framework_Module.Interfaces;
using UnityEngine;

namespace World_Module.WorldObjects
{
    /// <summary>
    /// Represents a player or enemy vehicle in the game.
    /// Handles health, movement, weapon firing, invincibility, and damage feedback.
    /// </summary>
    [RequireComponent(typeof(VehicleTinter))]
    internal sealed class Vehicle : WorldObject, IVehicle
    {
        [SerializeField] private VehicleExplosionView concreteVehicleExplosionView;
        [SerializeField] private Transform weaponSocketRoot;

        public AlignmentType AlignmentType { get; set; }
        public float MaxHealth => VehicleData.VehicleDefinition.MaxHealth;
        public float Health { get; private set; }
        public bool IsAlive => Health > 0;
        public bool IsReadyForRemoval => explosionComplete;
        private bool explosionComplete = false;
        public float Speed => currentSpeed;
        public float AttackRate => currentAttackRate;
        private int Armor => VehicleData.VehicleDefinition.Armor + armorUpgradeLevel;
        
        public Quaternion TransformRotation => transform.rotation;
        public VehicleData VehicleData { get; private set; }
        private WeaponSocket[] weaponSockets;
        private EventBus eventBus;
        private IVehicleExplosionView vehicleExplosionView;
        private IVehicleTinter vehicleTinter;
        private IConfigDatabase configDatabase;

        private float currentAttackRate = 0;
        private float currentSpeed = 0;
        private int armorUpgradeLevel = 0;

        public override void Awake()
        {
            base.Awake();
            vehicleExplosionView = concreteVehicleExplosionView;
            vehicleTinter = GetComponent<VehicleTinter>();
            weaponSockets = weaponSocketRoot.GetComponentsInChildren<WeaponSocket>();
        }

        public void SetArmorUpgradeLevel(int level)
        {
            armorUpgradeLevel = level;
        }

        //todo: may want to cache by type instead of linear search
        public IWeaponSocket GetSocketForType(WeaponType type)
        {
            foreach (var socket in weaponSockets)
            {
                if (socket.SupportsWeaponType(type))
                {
                    return socket;
                }
            }

            return null;
        }

        public void ModifyMovementSpeed(float movementSpeedAdjust, bool isAdditiveAdjustment)
        {
            if (VehicleData == null || VehicleData.VehicleDefinition == null)
            {
                currentSpeed = 0;
                DebugLogger.Log("Vehicle Data or Def is null. The vehicle has not been set properly!", LogCategory.World, LogLevel.Warning);
                return;
            }
            
            var newSpeed = isAdditiveAdjustment ? VehicleData.VehicleDefinition.Speed + movementSpeedAdjust : movementSpeedAdjust;
            currentSpeed = Mathf.Max(0, newSpeed);
        }

        public void ResetMovementSpeed()
        {
            if (VehicleData == null || VehicleData.VehicleDefinition == null)
            {
                currentSpeed = 0;
                DebugLogger.Log("Vehicle Data or Def is null. The vehicle has not been set properly!", LogCategory.World, LogLevel.Warning);
                return;
            }
            
            currentSpeed = Mathf.Max(0, VehicleData.VehicleDefinition.Speed);
        }
        
        public void ModifyAttackRateAdjustment(float attackRateAdjust, bool isAdditiveAdjustment)
        {
            if (VehicleData == null || VehicleData.VehicleDefinition == null)
            {
                currentAttackRate = 0;
                DebugLogger.Log("Vehicle Data or Def is null. The vehicle has not been set properly!", LogCategory.World, LogLevel.Warning);
                return;
            }
            
            var newAttackRate = isAdditiveAdjustment ? VehicleData.VehicleDefinition.BaseFireIntervalInSeconds + attackRateAdjust : attackRateAdjust;
            currentAttackRate = Mathf.Max(0, newAttackRate);
        }
        
        public void ResetAttackRate()
        {
            if (VehicleData == null || VehicleData.VehicleDefinition == null)
            {
                currentAttackRate = 0;
                DebugLogger.Log("Vehicle Data or Def is null. The vehicle has not been set properly!", LogCategory.World, LogLevel.Warning);
                return;
            }
            
            currentAttackRate = Mathf.Max(0, VehicleData.VehicleDefinition.BaseFireIntervalInSeconds);
        }

        public void Inject(EventBus eventBusService)
        {
            eventBus = eventBusService;
        }
        
        public void Set(VehicleData vehicleData)
        {
            VehicleData = vehicleData;
            Health = MaxHealth; 
            vehicleTinter.ResetTint();
            Collider.enabled = true;
            ResetMovementSpeed();
            ResetAttackRate();
        }
        
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        internal IVehicleController DebugController;
        public void DebugSetup(IVehicleController controller)
        {
            DebugController = controller;
        }
#endif

        public void Clear()
        {
            Health = 0;
            SetVelocity(Vector2.zero);
            Position = Vector3.zero;
            SetRotation(Quaternion.identity);
            transform.localScale = Vector3.zero;
            currentSpeed = 0;
            currentAttackRate = 0;
            explosionComplete = false;
            vehicleExplosionView.ResetView();
            Collider.enabled = false;
            vehicleTinter.ResetTint();
        }
        
        public void TakeDamage(float damage)
        {
            damage -= Armor;
            if (damage == 0)
                return;
            
            if (damage < 0)
            {
                DebugLogger.Log("Detected a negative damage value. Use Heal() to add health", LogCategory.World, LogLevel.Warning);
                return;
            }

            Health -= damage;
            eventBus.Publish(new DamageTakenEvent(this, damage));
            
            if (!IsAlive)
            {
                Collider.enabled = false;
                eventBus.Publish(new PlaySfxEvent(AudioSfxType.Explosion));
                vehicleExplosionView.ShowExplosion();
                eventBus.Publish(new WorldObjectKilledEvent(this));
            }
            else
            {
                var soundType = AlignmentType == AlignmentType.Friend ? AudioSfxType.DamageWarning1 : AudioSfxType.TakeDamage;
                eventBus.Publish(new PlaySfxEvent(soundType));
                vehicleTinter.TintOverTime(new Color(1, 0, 0, 1));
            }
        }
        
        public void OnExplosionComplete()
        {
            explosionComplete = true;
        }

        public void Heal(float healing)
        {
            if (healing == 0)
                return;
            
            if (healing < 0)
            {
                DebugLogger.Log("Detected a negative heal value. Use TakeDamage() to add damage", LogCategory.World, LogLevel.Warning);
                return;
            }
            
            Health += healing;
            eventBus.Publish(new HealEvent(this, healing));
        }


    }
}
