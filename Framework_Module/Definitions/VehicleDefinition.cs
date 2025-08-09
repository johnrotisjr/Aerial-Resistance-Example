using System;
using Framework_Module.Configs.Ai;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct VehicleDefinition
    {
        [SerializeField] private VehicleArchetype vehicleArchetype;
        [SerializeField] private AiBehaviorSequenceConfig aIBehaviorSequence;
        [SerializeField] private Sprite icon;
        [SerializeField] private float maxHealth;
        [SerializeField] private float speed;
        [SerializeField] private int armor;
        [SerializeField] private float fireRateInterval;
        [SerializeField] private bool isPlayerOnly;
        [SerializeField] private GameObject prefab;
        [SerializeField] string displayName;
        public string DisplayName => displayName;        
        public VehicleArchetype VehicleArchetype => vehicleArchetype;
        public AiBehaviorSequenceConfig AIBehaviorSequence => aIBehaviorSequence;
        public Sprite Icon => icon;
        public float MaxHealth => maxHealth;
        public float Speed => speed;
        public int Armor => armor;
        public float FireRateInterval => fireRateInterval;
        public bool IsPlayerOnly => isPlayerOnly;
        public GameObject Prefab => prefab;
        
        public VehicleDefinition( string displayName,
            VehicleArchetype vehicleArchetype,
            AiBehaviorSequenceConfig aIBehaviorSequence,
            Sprite icon,
            float maxHealth,
            float speed,
            int armor,
            float fireRateInterval,
            bool isPlayerOnly,
            GameObject prefab)
        {
            this.displayName = displayName;
            this.vehicleArchetype = vehicleArchetype;
            this.aIBehaviorSequence = aIBehaviorSequence;
            this.icon = icon;
            this.maxHealth = maxHealth;
            this.speed = speed;
            this.armor = armor;
            this.fireRateInterval = fireRateInterval;
            this.isPlayerOnly = isPlayerOnly;
            this.prefab = prefab;
        }
    }
}