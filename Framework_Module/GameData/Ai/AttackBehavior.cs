using Debug_Module;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Enums;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Framework_Module.GameData.Ai
{

    public abstract class AttackBehavior<TDef> : VehicleAiBehavior<TDef>, IAttackBehavior
        where TDef : AttackAiBehaviorDefinition
    {
        protected abstract WeaponType WeaponType { get; }
        protected TDef Def;
        protected IVehicle CurrentVehicle;
        private float fireRateRemainingCooldown = 0;
        protected bool CanFire => fireRateRemainingCooldown <= 0;
        protected void ResetFireCooldown()
        {
            if (Def == null || CurrentVehicle == null)
            {
                DebugLogger.Log("Definition or Vehicle is null for this attack behavior. That shouldn't happen!!", LogCategory.Ai, LogLevel.Warning);
                return;
            }
            var newAttackRate = Def.IsAdditiveAdjustment ? CurrentVehicle.AttackRate + Def.AttackRateAdjustment : Def.AttackRateAdjustment;
            fireRateRemainingCooldown = Mathf.Max(0, newAttackRate);
        }
        
        public override void Start(IVehicle vehicle)
        {
            CurrentVehicle = vehicle;
        }

        public override void End(IVehicle vehicle)
        {
            fireRateRemainingCooldown = 0;
            CurrentVehicle = null;
        }

        public override void Tick(float deltaTime, IVehicle vehicle)
        {
            if(fireRateRemainingCooldown > 0)
                fireRateRemainingCooldown = Mathf.Max(0, fireRateRemainingCooldown - Time.deltaTime);
        }
    }
}