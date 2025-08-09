using Framework_Module.Definitions;
using Framework_Module.Enums;

namespace Framework_Module.Interfaces
{
    public interface IWeapon : IWorldObject
    {
        public void Initialize(IWeaponBehaviorFactory factory);
        public AlignmentType AlignmentType { get; set; }
        public float Damage { get; }
        public WeaponType WeaponType { get; }
        public float Speed { get; }
        public bool HasCollided { get; set; }
        public WeaponDefinition WeaponDefinition { get; }
    }
}