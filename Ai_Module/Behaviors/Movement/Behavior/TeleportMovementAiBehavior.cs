using System;
using Framework_Module.Definitions;
using Framework_Module.Definitions.Behaviors;
using Framework_Module.Definitions.Behaviors.Movement;
using Framework_Module.GameData.Ai;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Behavior
{
    /// <summary>
    /// Teleports the vehicle to a position on screen
    /// </summary>
    [Serializable]
    public class TeleportMovementAiBehavior : MovementBehavior<TeleportMovementAiBehaviorDefinition>
    {
        private bool teleportComplete = false;
        public override void Tick(float deltaTime, IVehicle vehicle)
        {
            if (vehicle == null || teleportComplete)
                return;
            
            vehicle.Position = Def.TeleportPosition;
            RaiseCompletedMovementCycle();
            teleportComplete = true;
        }
        
        public override void End(IVehicle vehicle)
        {
            base.End(vehicle);
        }
        
        public override void Rebind(TeleportMovementAiBehaviorDefinition aiBehaviorDefinition)
        {
            Def = aiBehaviorDefinition;
        }
        
        public override void Start(IVehicle vehicle)
        {
            base.Start(vehicle);
        }

    }
}