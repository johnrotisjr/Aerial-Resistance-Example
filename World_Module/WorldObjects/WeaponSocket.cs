using System;
using System.Collections.Generic;
using Framework_Module.Enums;
using Framework_Module.Interfaces;
using UnityEngine;

namespace World_Module.WorldObjects
{
    public class WeaponSocket : MonoBehaviour, IWeaponSocket
    {
        [SerializeField] private WeaponType[] weaponTypes;
        HashSet<WeaponType> weaponTypesHash;
        public Vector3 Position => transform.position;

        private void Awake()
        {
            weaponTypesHash = new(weaponTypes);
        }

        public bool SupportsWeaponType(WeaponType type)
        {
            return weaponTypesHash.Contains(type);
        }
    }
}