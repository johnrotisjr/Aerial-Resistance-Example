using System;
using Debug_Module;
using Framework_Module.Core;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Extensions;
using Framework_Module.GameData.Instructions;
using Framework_Module.Interfaces;
using UnityEngine;
using World_Module.WorldObjects;

namespace World_Module
{
    /// <summary>
    /// Responsible for spawning vehicle and weapons using configuration data and prefab references.
    /// Supports directional alignment and velocity assignment for all spawned entities.
    /// </summary>

    public class WorldObjectSpawner : IWorldObjectSpawner
    {
        private readonly IConfigDatabase configDatabase;
        private readonly GameObjectPooler gameObjectPooler;
        private readonly IViewportBoundsProvider viewportBoundsProvider;
        private readonly EventBus eventBus;
        private readonly IWeaponBehaviorFactory behaviorFactory;

        public WorldObjectSpawner(IConfigDatabase configDatabase, GameObjectPooler gameObjectPooler, IViewportBoundsProvider viewportBoundsProvider, IWeaponBehaviorFactory behaviorFactory, EventBus eventBusService)
        {
            this.configDatabase = configDatabase;
            this.gameObjectPooler = gameObjectPooler;
            this.viewportBoundsProvider = viewportBoundsProvider;
            this.behaviorFactory = behaviorFactory;
            eventBus = eventBusService;
        }

        private Vector3 GetSpawnLocation(DirectionType edge, float edgePos, Vector3 spriteExtents)
        {
            var view = viewportBoundsProvider.GetPixelAlignedViewport(); // Rect in world units
            float t = Mathf.Clamp01(edgePos);

            // In-screen center ranges
            float cx = Mathf.Lerp(view.xMin + spriteExtents.x, view.xMax - spriteExtents.x, t);
            float cy = Mathf.Lerp(view.yMin + spriteExtents.y, view.yMax - spriteExtents.y, t);

            float x, y;

            switch (edge)
            {
                case DirectionType.Up:
                    x = cx;
                    y = view.yMax + spriteExtents.y; // just above
                    break;
                case DirectionType.Down:
                    x = cx;
                    y = view.yMin - spriteExtents.y; // just below
                    break;
                case DirectionType.Left:
                    x = view.xMin - spriteExtents.x; // just left
                    y = cy;
                    break;
                case DirectionType.Right:
                    x = view.xMax + spriteExtents.x; // just right
                    y = cy;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(edge), edge, null);
            }

            return new Vector3(x, y, 0f);
        }
        
        public IVehicle Spawn(SpawnVehicleInstruction vehicleInstruction, bool setFacingDirection = true)
        {
            if (vehicleInstruction.VehicleData == null)
            {
                DebugLogger.Log($"No data found for Vehicle", LogCategory.World, LogLevel.Error);
                return null;
            }

            var data = vehicleInstruction.VehicleData.VehicleDefinition;

            var vehicle = gameObjectPooler.Get<Vehicle>(vehicleInstruction.VehicleData.Guid);
            vehicle.Inject(eventBus);
            vehicle.SetRotation(Quaternion.identity);
            vehicle.SetVelocity(Vector2.zero);
            vehicle.Set(vehicleInstruction.VehicleData);
            var edge = vehicleInstruction.DirectionType;
            var edgePos = vehicleInstruction.SpawnEdgePos;
            var scale = data.Prefab.transform.localScale;
            if (setFacingDirection)
            {
                var isOnRight = (edge == DirectionType.Right || 
                                 (edge == DirectionType.Up && edgePos >= .5f) ||
                                 (edge == DirectionType.Down && edgePos >= .5f));
                float sign = isOnRight ? -1f : +1f;
                vehicle.transform.localScale = new Vector3(sign * Mathf.Abs(scale.x), scale.y, scale.z);
            }
            var vehicleInWorldUnits = vehicle.ActualSizeInWorldUnits();
            var halfVehicleInWorldUnits = vehicleInWorldUnits.Half();
            var pos = GetSpawnLocation(edge, edgePos, halfVehicleInWorldUnits);

            vehicle.Position = pos;
                
            return vehicle;
        }

        public IWeapon Spawn(WeaponType type, Vector2 position, Vector2 velocity, Quaternion rotation, AlignmentType alignmentType, IVehicle parentVehicle)
        {
            if (!configDatabase.GetWeaponDefinition(type, out var data))
            {
                DebugLogger.Log($"No data found for type: {type}", LogCategory.World, LogLevel.Error);
                return null;
            }
            
            var prefab = data.Prefab;
            if (prefab)
            {
                var weapon = gameObjectPooler.Get<Weapon>(data.Type.ToString());
                weapon.Initialize(behaviorFactory);
                weapon.SetRotation(rotation);
                weapon.AlignmentType = alignmentType;
                weapon.Set(data);
                weapon.SetVelocity(velocity);
                if(parentVehicle != null)
                    weapon.transform.SetParent(parentVehicle.Transform);
                weapon.transform.localPosition = Vector3.zero;

                var scale = prefab.transform.localScale;
                weapon.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
                weapon.Position = position;

                return weapon;
            }

            DebugLogger.Log($"No prefab found for type: {type}", LogCategory.World, LogLevel.Warning);
            return null;
        }

        public IPickup Spawn(SpawnPickupInstruction instruction)
        {
            var prefab = configDatabase.PickupPrefab;
            if (prefab)
            {
                var pickup = gameObjectPooler.Get<Pickup>(PrefabKey.Pickup.ToString());
                pickup.transform.localScale = prefab.transform.localScale;
                pickup.SetRotation(Quaternion.identity);
                pickup.SetVelocity(Vector2.left);

                if (!configDatabase.GetPickupDefinition(instruction.Type, out var data))
                {
                    DebugLogger.Log($"No data found for type: {instruction.Type}", LogCategory.World, LogLevel.Error);
                    return null;
                }

                pickup.Set(data);
                var pos = GetSpawnLocation(instruction.DirectionType, instruction.SpawnEdgePos,
                    pickup.ActualSizeInWorldUnits().Half());
                pickup.Position = pos;
                return pickup;
            }

            DebugLogger.Log($"No prefab found for type: {instruction.Type}", LogCategory.World, LogLevel.Warning);
            return null;
        }

        public void Despawn<T>(string key, T value) where T : IWorldObject, IClearable
        {
            value.Clear();
            gameObjectPooler.Release(key, value.GameObject);
        }
        
        public void Initialize()
        {
 
        }

        public void Shutdown()
        {
 
        }
    }
}
