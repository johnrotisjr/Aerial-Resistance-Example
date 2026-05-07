using Debug_Module;
using Framework_Module;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Interfaces;
using UnityEngine;
using World_Module.WorldObjects;

namespace World_Module
{
    /// <summary>
    /// Handles collision logic between vehicle and weapons.
    /// Validates collision layers and routes damage through the damage handler.
    /// </summary>

    public class CollisionResolver : IGameService
    {
        private readonly IViewportBoundsProvider viewportBoundsProvider;
        private readonly IPlayerController playerController;
        private Rect ViewPort => viewportBoundsProvider.GetPixelAlignedViewport();
        
        public CollisionResolver(IViewportBoundsProvider camController, IPlayerController playerControllerService)
        {
            viewportBoundsProvider = camController;
            playerController = playerControllerService;
        }

        public void HandleCollision(IWorldObject worldObject, IWorldObject other)
        {
            //Ignore out of view collisions
            if (!ViewPort.Overlaps(worldObject.SpriteBoundsRect) || !ViewPort.Overlaps(other.SpriteBoundsRect))
                return;
            
            var vehicle = worldObject as Vehicle;
            if (vehicle && other is Vehicle otherVehicle && vehicle.AlignmentType != otherVehicle.AlignmentType)
            {
                vehicle.TakeDamage(otherVehicle.MaxHealth);
                DebugLogger.Log("Collision vehicle occured", LogCategory.World, LogLevel.Log);
            }
            else if (vehicle && other is Weapon weapon && weapon.AlignmentType != vehicle.AlignmentType)
            {
                vehicle.TakeDamage(weapon.Damage);
                weapon.HandleCollision();
                DebugLogger.Log("Collision with weapon occured", LogCategory.World, LogLevel.Log);
            }
            else if (vehicle && other is Pickup pickup && ReferenceEquals(vehicle, playerController.ControlledVehicle))
            {
                pickup.Apply();
            }
        }

        public void Initialize()
        {
 
        }

        public void Shutdown()
        {
 
        }
    }
}