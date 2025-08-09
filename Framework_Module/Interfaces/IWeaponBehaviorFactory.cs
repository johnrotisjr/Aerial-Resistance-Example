using Framework_Module.Configs.Ai;
using Framework_Module.Enums;

namespace Framework_Module.Interfaces
{
    public interface IWeaponBehaviorFactory
    {
        public IBehavior GetBehavior(WeaponType type);
    }
}