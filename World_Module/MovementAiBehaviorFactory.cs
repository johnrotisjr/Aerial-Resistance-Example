using System;
using Ai_Module.Behaviors.Movement.Behavior;
using Debug_Module;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Definitions.Behaviors.Movement;
using Framework_Module.Enums;
using Framework_Module.Interfaces;

namespace World_Module
{
    /// <summary>
    /// </summary>
    public class MovementAiBehaviorFactory : IMovementAiBehaviorFactory
    {
        //TODO: Inject dependency
        public MovementAiBehaviorFactory()
        {
 
        }
        
        //TODO: use pooling
        public IMovementBehavior GetBehavior(MovementAiBehaviorDefinition def)
        {
            if (def == null)
            {
                DebugLogger.Log("Definition is null, cannot get behavior.", LogCategory.Ai, LogLevel.Warning);
                return null;
            }

            switch (def.MovementType)
            {
                case AiMovementType.None:
                    return null;
                case AiMovementType.Linear:
                   var linearMovementBehavior = new LinearMovementAiBehavior();
                   linearMovementBehavior.Rebind((LinearMovementAiBehaviorDefinition)def);
                    return linearMovementBehavior;
                case AiMovementType.Wobble:
                    var wobbleMovementBehavior = new WobbleMovementAiBehavior();
                    wobbleMovementBehavior.Rebind((WobbleMovementAiBehaviorDefinition)def);
                    return wobbleMovementBehavior;
                case AiMovementType.Orbit:
                    var orbitMovementBehavior = new OrbitMovementAiBehavior();
                    orbitMovementBehavior.Rebind((OrbitMovementAiBehaviorDefinition)def);
                    return orbitMovementBehavior;
                case AiMovementType.Stalker:
                    var stalkerMovementBehavior = new StalkerMovementAiBehavior();
                    stalkerMovementBehavior.Rebind((StalkerMovementAiBehaviorDefinition)def);
                    return stalkerMovementBehavior;
                case AiMovementType.Kamikaze:
                    var kamikazeMovementBehavior = new KamikazeMovementAiBehavior();
                    kamikazeMovementBehavior.Rebind((KamikazeMovementAiBehaviorDefinition)def);
                    return kamikazeMovementBehavior;
                case AiMovementType.MoveToPosition:
                    var moveToPositionMovementBehavior = new MoveToPositionMovementAiBehavior();
                    moveToPositionMovementBehavior.Rebind((MoveToPositionMovementAiBehaviorDefinition)def);
                    return moveToPositionMovementBehavior;
                case AiMovementType.Arching:
                    var archingMovementBehavior = new ArchingMovementAiBehavior();
                    archingMovementBehavior.Rebind((ArchingMovementAiBehaviorDefinition)def);
                    return archingMovementBehavior;
                case AiMovementType.Maintain:
                    var maintainMovementBehavior = new MaintainMovementAiBehavior();
                    maintainMovementBehavior.Rebind((MaintainMovementAiBehaviorDefinition)def);
                    return maintainMovementBehavior;
                case AiMovementType.Patrol:
                    var patrolMovementBehavior = new PatrolMovementAiBehavior();
                    patrolMovementBehavior.Rebind((PatrolMovementAiBehaviorDefinition)def);
                    return patrolMovementBehavior;
                case AiMovementType.Teleport:
                    var teleportMovementAiBehavior = new TeleportMovementAiBehavior();
                    teleportMovementAiBehavior.Rebind((TeleportMovementAiBehaviorDefinition)def);
                    return teleportMovementAiBehavior;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IAiBehavior<IVehicle> GetBehavior(AiBehaviorDefinition def)
        {
            return GetBehavior(def as MovementAiBehaviorDefinition);
        }
    }
}