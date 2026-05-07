using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.GameData.Ai;
using Framework_Module.Interfaces;
using UnityEngine;

namespace World_Module.WeaponBehaviors
{
    public class ThrustWeaponMovementBehavior : WeaponAiBehavior
    {
        private bool isEngorging = true;
        private float accumulatedTime;
        private const float timeToEnd = 0.5f;
        public override void Tick(float deltaTime, IWeapon weapon)
        {
            accumulatedTime += deltaTime;
            var t = accumulatedTime / timeToEnd;
            if (accumulatedTime >= timeToEnd)
            {
                t = 1;
                isEngorging = !isEngorging;
                accumulatedTime = 0;
            }
            
            float a = isEngorging ? 1 : 3;
            float b = isEngorging ? 3 : 1;
            weapon.Scale(Mathf.Lerp(a, b, t), weapon.LocalScale.y, weapon.LocalScale.z);
        }

        public override void End(IWeapon weapon)
        {
     
        }

        public override void Start(IWeapon weapon)
        {
            isEngorging = true;
        }
 
    }
}