using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Interfaces
{
    public interface IWeaponSocket
    {
        public Vector3 Position { get; }
        public bool SupportsWeaponType(WeaponType type);
    }
}