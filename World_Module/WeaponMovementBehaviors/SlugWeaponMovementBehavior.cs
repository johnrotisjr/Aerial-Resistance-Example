using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.GameData.Ai;
using Framework_Module.Interfaces;
using UnityEngine;

namespace World_Module.WeaponBehaviors
{
    public class SlugWeaponMovementBehavior : WeaponAiBehavior
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