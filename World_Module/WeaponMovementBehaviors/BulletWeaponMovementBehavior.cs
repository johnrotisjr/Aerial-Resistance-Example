using Framework_Module.GameData.Ai;
using Framework_Module.Interfaces;

namespace World_Module.WeaponMovementBehaviors
{
    public class BulletWeaponMovementBehavior : WeaponAiBehavior
    {
        public override void Tick(float deltaTime, IWeapon weapon)
        {
            weapon.SetVelocity(weapon.Velocity);
        }

        public override void End(IWeapon weapon)
        {
     
        }

        public override void Start(IWeapon weapon)
        {
            
        }
 
    }
}