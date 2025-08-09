using Framework_Module.Enums;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Framework_Module.Event.Gameplay
{
    //TODO: Cache Events
    public class SpawnWeaponEvent : IGameEvent
    {
        public readonly Vector2 Position;
        public readonly Vector2 Direction;
        public readonly Quaternion Rotation;
        public readonly AlignmentType AlignmentType;
        public readonly WeaponType WeaponType;

        public SpawnWeaponEvent(WeaponType weaponType, Vector2 position, Vector2 direction, Quaternion rotation, AlignmentType alignmentType)
        {
            WeaponType = weaponType;
            Position = position;
            Direction = direction;
            Rotation = rotation;
            AlignmentType = alignmentType;
        }
    }
}