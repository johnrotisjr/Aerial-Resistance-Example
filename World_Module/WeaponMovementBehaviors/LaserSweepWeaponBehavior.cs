using Framework_Module.GameData.Ai;
using Framework_Module.Interfaces;
using UnityEngine;

namespace World_Module.WeaponBehaviors
{
    public class LaserSweepWeaponBehavior : WeaponAiBehavior
    {
        private readonly float speedDegPerSec = 120f;  
        private readonly float center = 0f;
        private readonly float amplitude = 180f;
        private float totalMovementInDegrees = 0f;
        
        public override void Tick(float deltaTime, IWeapon weapon)
        {
            float step = deltaTime * speedDegPerSec;
            totalMovementInDegrees += step;
            float z = center + Mathf.PingPong(totalMovementInDegrees, amplitude);
            var quaternion = Quaternion.AngleAxis(z, Vector3.forward);
            weapon.SetRotation(quaternion);
        }

        public override void End(IWeapon weapon)
        {
        }

        public override void Start(IWeapon weapon)
        {
            totalMovementInDegrees = 0f;
        }
 
    }
}