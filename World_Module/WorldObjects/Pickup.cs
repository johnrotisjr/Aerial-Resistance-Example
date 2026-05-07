using System;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;

namespace World_Module.WorldObjects
{
    internal class Pickup : WorldObject, IPickup
    {
        private SpriteRenderer spriteRenderer;
        private PickupDefinition pickupDefinition;

        public float Value => pickupDefinition.Value;
        private IPlayerController controller;
        private EventBus eventBus;

        public override void Awake()
        {
            base.Awake();
            controller = Services.Get<IPlayerController>();
            eventBus = Services.Get<EventBus>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Apply()
        {
            switch (pickupDefinition.Type)
            {
                case PickupType.Health:
                    controller.ControlledVehicle.Heal(pickupDefinition.Value);
                    break;
                case PickupType.Power:
                    controller.AddPower(pickupDefinition.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            eventBus.Publish(new PickupCollectedEvent(this));
            eventBus.Publish(new PlaySfxEvent(AudioSfxType.Collect));
        }
        
        public void Set(PickupDefinition definition)
        {
            pickupDefinition = definition;
            spriteRenderer.sprite = definition.Sprite;
        }

        public void Clear()
        {
            pickupDefinition = new PickupDefinition();
        }
    }
}