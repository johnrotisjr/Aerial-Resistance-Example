using System.Collections.Generic;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Interfaces
{
    /// <summary>
    /// Manages the flow of a mission including enemy and boss spawns.
    /// Tracks mission progress and coordinates start/reset behavior.
    /// </summary>

    public interface IMissionManager : IGameService
    {
        public bool HasCompletedPrimaryObjectives();
        public void Reset();
        public IVehicle SpawnPlayer();
        public IWeapon SpawnWeapon(WeaponType weaponType, Vector2 position, Vector2 velocity, Quaternion rotation, AlignmentType alignment);
        public List<IVehicleController> CheckEnemySpawn(float totalDistanceFlown);
        public IVehicleController CheckBossSpawn(float totalDistanceFlown);
        public List<IPickup> CheckPickupSpawn(float totalDistanceFlown);
    }
}
