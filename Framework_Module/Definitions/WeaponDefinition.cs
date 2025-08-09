using System;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct WeaponDefinition
    {
        [SerializeField] private WeaponType type;
        [SerializeField] private Sprite icon;
        [SerializeField] private float damage;
        [SerializeField] private float speed;
        [SerializeField] private bool canBePurchased;
        [SerializeField] private GameObject prefab;
        [SerializeField] string displayName;
        public string DisplayName => displayName;        
        public WeaponType Type => type;
        public Sprite Icon => icon;
        public float Damage => damage;
        public float Speed => speed;
        public bool CanBePurchased => canBePurchased;
        public GameObject Prefab => prefab;
        
        public WeaponDefinition(
            string displayName,
            WeaponType type,
            Sprite icon,
            float damage,
            float speed,
            bool canBePurchased,
            GameObject prefab)
        {
            this.displayName = displayName;
            this.type = type;
            this.icon = icon;
            this.damage = damage;
            this.speed = speed;
            this.canBePurchased = canBePurchased;
            this.prefab = prefab;
        }
    }
}