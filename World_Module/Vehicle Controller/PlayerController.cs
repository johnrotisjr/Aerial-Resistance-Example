using System;
using System.Runtime.CompilerServices;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.Combat;
using Framework_Module.Event.Gameplay;
using Framework_Module.Event.Ui;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;
using World_Module.WorldObjects;

[assembly: InternalsVisibleTo("Game.Tests")]
namespace World_Module.Vehicle_Controller
{
    /// <summary>
    /// Handles player input, movement, and vehicle-related gameplay logic.
    /// Acts as the main controller for the player during missions.
    /// </summary>
    public class PlayerController : VehicleController, IPlayerController
    {
        private Vector2 direction;
        private Rect ViewRect => viewportBoundsProvider.GetPixelAlignedViewport();
        public float Speed => ControlledVehicle?.Speed ?? 0;
        private readonly EventBus eventBus;
        private readonly IGameData gameData;
        private readonly IViewportBoundsProvider viewportBoundsProvider;
        private bool isFiring = false;
        private int currentWeaponIndex;
        private float fireRateRemainingCooldown = 0;

        public override void AssignNewVehicle(IVehicle vehicle)
        {
            base.AssignNewVehicle(vehicle);
            vehicle.ModifyAttackRateAdjustment(0, true);
        }

        public override void UnassignVehicle()
        {
            base.UnassignVehicle();
            ControlledVehicle?.ResetAttackRate();
        }
        
        private bool CanFire()
        {
            return fireRateRemainingCooldown <= 0;
        }

        public WeaponDefinition CurrentlyEquippedWeapon()
        {
            var selectedWeaponData = gameData.TransientPlayerData.SelectedWeaponData;
            if (selectedWeaponData == null || currentWeaponIndex < 0 || currentWeaponIndex >= selectedWeaponData.Count)
                return null;
            
            return gameData.TransientPlayerData.SelectedWeaponData[currentWeaponIndex];
        }

        public void ProcessInput(DigitalInputActionType type, bool isPressed)
        {
            switch (type)
            {
                case DigitalInputActionType.FireGun:
                    if(!isPressed && isFiring)
                        EndFireGun();
                    else
                        FireGun();
                    break;
                case DigitalInputActionType.FireWeapon:
                    FireWeapon();
                    break;
                case DigitalInputActionType.ChangeWeapon:
                    ChangeWeapon();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void ProcessInput(AxisInputActionType type, Vector2 inputValue)
        {
            if(type == AxisInputActionType.Move)
                Move(inputValue);
        }

        public PlayerController(IGameData data, IViewportBoundsProvider viewportBoundsProvider, EventBus eventBus)
        {
            gameData = data;
            this.viewportBoundsProvider = viewportBoundsProvider;
            this.eventBus = eventBus;
        }
        
        public void Initialize()
        {
            eventBus.Subscribe<WorldObjectKilledEvent>(OnWorldObjectDestroyed);
            eventBus.Subscribe<RestartMissionEvent>(OnRestartMissionEvent);
            eventBus.Subscribe<DamageTakenEvent>(OnDamageTaken);
            eventBus.Subscribe<HealEvent>(OnHeal);

        }
        
        public void Shutdown()
        {
            eventBus.Unsubscribe<WorldObjectKilledEvent>(OnWorldObjectDestroyed);
            eventBus.Unsubscribe<RestartMissionEvent>(OnRestartMissionEvent);
            eventBus.Unsubscribe<DamageTakenEvent>(OnDamageTaken);
            eventBus.Unsubscribe<HealEvent>(OnHeal);
        }

        private void FireWeapon()
        {
            var tempData = gameData.TransientPlayerData;
            if (tempData?.SelectedWeaponData == null || tempData.SelectedWeaponData.Count <= 0)
                return;

            var spriteBoundsHalf = new Vector3(ControlledVehicle.SpriteBounds.size.x * 0.5f, 0);
            var directionScalar = ControlledVehicle.IsFacingRight ? 1 : -1;
            var spriteOffset = directionScalar * spriteBoundsHalf;
            var socket = ControlledVehicle.GetSocketForType(tempData.SelectedWeaponData[currentWeaponIndex].Type);
            var position = socket?.Position ?? ControlledVehicle.Position + spriteOffset;

            eventBus.Publish(new QueueSpawnWeaponEvent(tempData.SelectedWeaponData[currentWeaponIndex].Type, position, 
                ControlledVehicle.IsFacingRight? Vector2.right : Vector2.left, ControlledVehicle.WorldRotation, 
                AlignmentType.Friend));
            
            eventBus.Publish(new UpdateHudEvent());
        }

        private void ChangeWeapon()
        {
            var tempData = gameData.TransientPlayerData;
            if(tempData.SelectedWeaponData == null)
                return;
            
            currentWeaponIndex++;
            if (currentWeaponIndex >= tempData.SelectedWeaponData.Count)
                currentWeaponIndex = 0;
            
            eventBus.Publish(new UpdateHudEvent());
        }
        
        private void FireGun()
        {
            isFiring = true;
        }
        
        private void EndFireGun()
        {
            isFiring = false;
        }
        
        private void OnHeal(HealEvent e)
        {
            if (e.WorldObject is IVehicle a && ControlledVehicle == a)
                eventBus.Publish(new UpdateHudEvent());
        }
        
        private void OnDamageTaken(DamageTakenEvent e)
        {
            if (e.WorldObject is IVehicle a && ControlledVehicle == a)
                eventBus.Publish(new UpdateHudEvent());
        }
        
        public void AddPower(float powerIncrease)
        {
            gameData.TransientPlayerData.AddFirepower(powerIncrease);
            eventBus.Publish(new UpdateHudEvent());
        }

        public void Move(Vector2 newDirection)
        {
            if (ControlledVehicle != null)
            {
                var normalizedDirection = newDirection;
                if (normalizedDirection.sqrMagnitude > 1)
                    normalizedDirection.Normalize();
                
                direction = normalizedDirection;
            }
        }

        public override void Update()
        {
            if (ControlledVehicle == null) 
                return;
            
            fireRateRemainingCooldown = Mathf.Max(0, fireRateRemainingCooldown - Time.deltaTime);

            if (isFiring && CanFire())
            {
                FireBullet();
            }
        }

        private void FireBullet()
        {
            if (ControlledVehicle == null)
                return;
            
            fireRateRemainingCooldown = ControlledVehicle.VehicleData.VehicleDefinition.BaseFireIntervalInSeconds;
            var socket = ControlledVehicle.GetSocketForType(WeaponType.Bullet);
            var position = socket?.Position ?? ControlledVehicle.Position;
            eventBus.Publish(new QueueSpawnWeaponEvent(WeaponType.Bullet, position, 
                Vector2.right * gameData.TransientPlayerData.FirePower, ControlledVehicle.WorldRotation, AlignmentType.Friend));
        }

        public override void FixedUpdate()
        {
            if (ControlledVehicle != null)
            {
                Vector2 safeDirection = GetClampedDirection();
                ControlledVehicle.SetVelocity(safeDirection * Speed);
                
                var pos = ControlledVehicle.Position;
                var boundsSize = ControlledVehicle.SpriteBounds.size;
                var halfBounds = new Vector2(boundsSize.x * 0.5f, boundsSize.y * 0.5f);
                ControlledVehicle.Position = GetClampedPosition(pos, halfBounds);
            }
        }
        
        private Vector2 GetClampedDirection()
        {
            var currentPos = ControlledVehicle.Position;
            var boundsSize = ControlledVehicle.SpriteBounds.size;
            var halfBounds = new Vector2(boundsSize.x * 0.5f, boundsSize.y * 0.5f);

            var size = Services.Get<IHud>().GetHudPpuScaledSize();
            var viewMin = ViewRect.min + halfBounds;
            var viewMax = ViewRect.max - halfBounds;
            viewMax.y += size.y;
            
            Vector2 desiredDirection = direction;
            Vector2 attemptedNext = currentPos + (Vector3)(desiredDirection * (Speed * Time.deltaTime));

            if (attemptedNext.x < viewMin.x || attemptedNext.x > viewMax.x)
                desiredDirection.x = 0;
            if (attemptedNext.y < viewMin.y || attemptedNext.y > viewMax.y)
                desiredDirection.y = 0;

            return desiredDirection;
        }
        
        private Vector3 GetClampedPosition(Vector3 position, Vector3 offset)
        {
            var worldViewBounds = viewportBoundsProvider.WorldViewBounds;
            var size = Services.Get<IHud>().GetHudPpuScaledSize();
            Vector2 min = worldViewBounds.min + offset;
            Vector2 max = worldViewBounds.max - offset;
            max.y += size.y;
            
            return new Vector3(
                Mathf.Clamp(position.x, min.x, max.x),
                Mathf.Clamp(position.y, min.y, max.y), 
                position.z
            );
        }
        
        private void OnWorldObjectDestroyed(WorldObjectKilledEvent e)
        {
            if (e.WorldObject is not Vehicle vehicle || vehicle.AlignmentType != AlignmentType.Foe)
                return;

            eventBus.Publish(new UpdateHudEvent());
        }

        private void OnRestartMissionEvent(RestartMissionEvent e)
        {
            direction = Vector2.zero;
            eventBus.Publish(new UpdateHudEvent());
        }
    }
}
