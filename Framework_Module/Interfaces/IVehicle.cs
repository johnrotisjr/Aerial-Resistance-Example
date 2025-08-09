using Framework_Module.Definitions;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Interfaces
{
    public interface IVehicle : IWorldObject
    {
        public bool CanFireGun { get; }
        public void ResetFireCooldown();
        public void AdjustFireCooldown(float amount);
        public AlignmentType AlignmentType { get; set; }
        public VehicleArchetype VehicleArchetype { get; }
        public float MaxHealth { get; }
        public float Health { get; }
        public bool IsAlive { get; }
        public float Speed { get; }
        public Quaternion Rotation { get; }
        public VehicleDefinition VehicleDefinition { get; }
        public void SetArmorUpgradeLevel(int level);
        public void TakeDamage(float damage);
        public void Heal(float healing);
        public void DebugSetup(IVehicleController controller);
        public IWeaponSocket GetSocketForType(WeaponType type);
    }
}
