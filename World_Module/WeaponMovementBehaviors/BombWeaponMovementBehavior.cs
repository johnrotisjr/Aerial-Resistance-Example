using Framework_Module.Definitions;
using Framework_Module.GameData.Ai;
using Framework_Module.Interfaces;
using UnityEngine;

namespace World_Module.WeaponMovementBehaviors
{
    public class BombWeaponMovementBehavior : WeaponAiBehavior
    {
        public override void Tick(float deltaTime, IWeapon weapon)
        {
            weapon.SetVelocity(Vector2.down * weapon.Speed);
        }

        public override void End(IWeapon weapon)
        {

        }

        public override void Start(IWeapon weapon)
        {
            
        }
 
    }
}