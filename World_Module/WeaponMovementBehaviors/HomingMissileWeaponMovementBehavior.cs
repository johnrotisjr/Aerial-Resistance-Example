using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using Framework_Module.GameData.Ai;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;
using World_Module.WorldStates;

namespace World_Module.WeaponBehaviors
{
    public class HomingMissileWeaponMovementBehavior : WeaponAiBehavior
    {
        private readonly IWorldStateManager worldStateManager;
        private const float TurnSpeedDegPerSec = 1000;
        private float lifetime = 0;
        
        public HomingMissileWeaponMovementBehavior(IWorldStateManager worldStateManager)
        {
            this.worldStateManager = worldStateManager;
        }

        public override void Tick(float deltaTime, IWeapon weapon)
        {
            lifetime += deltaTime;

            if (worldStateManager.CurrentState is not PlayWorldState playWorldState)
                return;

            var vehicle = playWorldState.GetClosestEnemy(weapon);
            if (vehicle == null || lifetime < .2f)
            {
                return;
            }

            // Target direction in the XY plane
            Vector2 toEnemy = (Vector2)(vehicle.Position - weapon.Position);
            if (toEnemy.sqrMagnitude < Mathf.Epsilon)
                return;

            Vector2 dirToEnemy = toEnemy.normalized;
            float targetAng = Mathf.Atan2(dirToEnemy.y, dirToEnemy.x) * Mathf.Rad2Deg;

            // Read current facing angle. Replace with your getter if different.
            float currentAng = weapon.WorldRotation.eulerAngles.z;

            // Smoothly rotate toward the target. TurnSpeedDegPerSec is a cap per second.

            float t = Mathf.Clamp01(TurnSpeedDegPerSec * Time.deltaTime / 180f); // normalized blend
            float ang = Mathf.LerpAngle(currentAng, targetAng, t);

            // Build a direction from the new angle and set velocity
            Vector2 dir = new Vector2(Mathf.Cos(ang * Mathf.Deg2Rad), Mathf.Sin(ang * Mathf.Deg2Rad));

            weapon.SetVelocity(new Vector3(dir.x, dir.y, 0f) * weapon.Speed);
            weapon.SetRotation(Quaternion.Euler(0f, 0f, ang));
        }


        public override void End(IWeapon weapon)
        {
            lifetime = 0;
        }

        public override void Start(IWeapon weapon)
        {
            lifetime = 0;
        }
 
    }
}