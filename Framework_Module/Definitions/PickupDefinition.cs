using System;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct PickupDefinition
    {
        [SerializeField] private PickupType type;
        [SerializeField] private Sprite sprite;
        [SerializeField] private float value;
        [SerializeField] string displayName;
        public string DisplayName => displayName;        
        public PickupType Type => type;
        public Sprite Sprite => sprite;
        public float Value => value;
        
        public PickupDefinition(string displayName, PickupType type, Sprite sprite, float value)
        {
            this.displayName = displayName;
            this.type = type;
            this.sprite = sprite;
            this.value = value;
        }
    }
}