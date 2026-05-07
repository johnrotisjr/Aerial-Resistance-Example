using Framework_Module.Enums;
using Framework_Module.GameData.Data;

namespace Framework_Module.Interfaces
{
    public interface IVehicle : IWorldObject, IPooled<VehicleData>
    {
        public void ModifyMovementSpeed(float movementSpeedAdjust, bool isAdditiveAdjustment);
        public void ResetMovementSpeed();
        public void ModifyAttackRateAdjustment(float attackRateAdjust, bool isAdditiveAdjustment);
        public void ResetAttackRate();
        public AlignmentType AlignmentType { get; set; }
        public float MaxHealth { get; }
        public float Health { get; }
        public bool IsAlive { get; }
        public bool IsReadyForRemoval { get; }
        public float Speed { get; }
        public float AttackRate { get; }
        public VehicleData VehicleData { get; }
        public VehicleArchetype Archetype => VehicleData.VehicleDefinition.Archetype;
        public VehicleCategory Category => VehicleData.VehicleDefinition.Category;
        public VehicleRole Role => VehicleData.VehicleDefinition.Role;
        public VehicleTier Tier => VehicleData.VehicleDefinition.Tier;
        public void SetArmorUpgradeLevel(int level);
        public void TakeDamage(float damage);
        public void Heal(float healing);
        public void DebugSetup(IVehicleController controller);
        public IWeaponSocket GetSocketForType(WeaponType type);
    }
}
