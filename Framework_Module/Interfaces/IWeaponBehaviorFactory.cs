using Framework_Module.Enums;

namespace Framework_Module.Interfaces
{
    public interface IWeaponBehaviorFactory
    {
        public IWeaponBehavior GetBehavior(WeaponType type);
    }
}