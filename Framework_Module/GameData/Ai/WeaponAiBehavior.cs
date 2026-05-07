using Framework_Module.Definitions;
using Framework_Module.Interfaces;

namespace Framework_Module.GameData.Ai
{
    public abstract class WeaponAiBehavior : IWeaponBehavior
    {
        public abstract void Tick(float deltaTime, IWeapon worldObject);
        public abstract void End(IWeapon worldObject);
        public abstract void Start(IWeapon worldObject);
    }
}