using System;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Extensions
{
 
    public static class FrameworkExtensions
    {
        public static Rect BoundsToRect(this Bounds bounds)
        {
            var center = bounds.center;
            var extents = bounds.extents;
            return new Rect(
                center.x - extents.x,
                center.y - extents.y,
                extents.x * 2f,
                extents.y * 2f
            );
        }

        public static PrefabKey GetPrefabKey(this VehicleArchetype type)
        {
            switch (type)
            {
                case VehicleArchetype.F14:
                    return PrefabKey.F14;
                case VehicleArchetype.F15:
                    return PrefabKey.F15;
                case VehicleArchetype.H10:
                    return PrefabKey.H10;
                case VehicleArchetype.T10:
                    return PrefabKey.T10;
                case VehicleArchetype.B10:
                    return PrefabKey.B10;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        public static PrefabKey GetPrefabKey(this WeaponType type)
        {
            switch (type)
            {
                case WeaponType.Bullet:
                    return PrefabKey.Bullet;
                case WeaponType.HomingMissile:
                    return PrefabKey.HomingMissile;
                case WeaponType.Slug:
                    return PrefabKey.Slug;
                case WeaponType.Bomb:
                    return PrefabKey.Bomb;
                case WeaponType.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}