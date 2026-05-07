using System;
using Framework_Module.Enums;
using Framework_Module.GameData.Ai;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework_Module.Definitions
{
    [Serializable]
    public class VehicleDefinition
    {
        [SerializeField] private VehicleCategory category;
        [SerializeField] private VehicleArchetype archetype;
        [SerializeField] private VehicleRole role;
        [SerializeField] private VehicleTier tier;
        [SerializeField] private AiBehaviorSequenceConfig aIBehaviorSequence;
        [SerializeField] private Sprite icon;
        [SerializeField] private float maxHealth;
        [SerializeField] private float speed;
        [SerializeField] private int armor;
        [FormerlySerializedAs("shotIntervalInSeconds")] 
        [SerializeField] private float baseFireIntervalInSeconds;
        [SerializeField] private bool isPurchasable;
        [SerializeField] private GameObject prefab;
        [SerializeField] string displayName;
        public string DisplayName => displayName;
        
        public VehicleArchetype Archetype => archetype;
        public VehicleCategory Category => category;
        public VehicleRole Role => role;
        public VehicleTier Tier => tier;
        public AiBehaviorSequenceConfig AIBehaviorSequence => aIBehaviorSequence;
        public Sprite Icon => icon;
        public float MaxHealth => maxHealth;
        public float Speed => speed;
        public int Armor => armor;
        public float BaseFireIntervalInSeconds => baseFireIntervalInSeconds;
        public bool IsPurchasable => isPurchasable;
        public GameObject Prefab => prefab;
        
        public VehicleDefinition(string displayName,
            VehicleArchetype archetype,
            VehicleCategory category,
            VehicleRole role,
            VehicleTier tier,
            AiBehaviorSequenceConfig aIBehaviorSequence,
            Sprite icon,
            float maxHealth,
            float speed,
            int armor,
            float baseFireIntervalInSeconds,
            bool isPurchasable,
            GameObject prefab)
        {
            this.displayName = displayName;
            this.archetype = archetype;
            this.category = category;
            this.role = role;
            this.tier = tier;
            this.aIBehaviorSequence = aIBehaviorSequence;
            this.icon = icon;
            this.maxHealth = maxHealth;
            this.speed = speed;
            this.armor = armor;
            this.baseFireIntervalInSeconds = baseFireIntervalInSeconds;
            this.isPurchasable = isPurchasable;
            this.prefab = prefab;
        }
    }
}