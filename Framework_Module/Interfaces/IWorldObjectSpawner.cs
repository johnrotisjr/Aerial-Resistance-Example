using Framework_Module.Enums;
using Framework_Module.GameData.Instructions;
using UnityEngine;

namespace Framework_Module.Interfaces
{
 

    public interface IWorldObjectSpawner : IGameService
    {
        public IVehicle Spawn(SpawnVehicleInstruction vehicleInstruction, bool setFacingDirection = true);
        public IWeapon Spawn(WeaponType type, Vector2 position, Vector2 velocity, Quaternion rotation,
            AlignmentType alignmentType, IVehicle parentVehicle);
        public IPickup Spawn(SpawnPickupInstruction instruction);
        public void Despawn<T>(string key, T value) where T : IWorldObject, IClearable;
    }
}
