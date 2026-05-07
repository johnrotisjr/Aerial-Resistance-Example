using Framework_Module.Enums;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Framework_Module.Event.Gameplay
{
    //TODO: Cache Events
    public class WeaponSpawnedEvent : IGameEvent
    {
        public readonly AlignmentType AlignmentType;
        public readonly IWeapon Weapon;
        public readonly IVehicle ParentVehicle;

        public WeaponSpawnedEvent(IWeapon weapon, IVehicle parentVehicle, AlignmentType alignmentType)
        {
            Weapon = weapon;
            ParentVehicle = parentVehicle;
            AlignmentType = alignmentType;
        }
    }
}