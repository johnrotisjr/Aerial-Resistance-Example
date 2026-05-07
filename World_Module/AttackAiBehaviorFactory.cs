using System;
using Ai_Module.Behaviors.Attack.Behavior;
using Debug_Module;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Enums;
using Framework_Module.Interfaces;

namespace World_Module
{
    /// <summary>
    /// </summary>
    public class AttackAiBehaviorFactory : IAttackAiBehaviorFactory
    {
        //TODO: Inject dependency
        public AttackAiBehaviorFactory()
        {
 
        }
        
        //TODO: use pooling
        public IAttackBehavior GetBehavior(AttackAiBehaviorDefinition def)
        {
            if (def == null)
            {
                DebugLogger.Log("Definition is null, cannot get behavior.", LogCategory.Ai, LogLevel.Warning);
                return null;
            }
 
            switch (def.AttackType)
            {
                case AiAttackType.None:
                    return null;
                case AiAttackType.Straight:
                    var straightAttackAiBehavior = new StraightAttackAiBehavior();
                    straightAttackAiBehavior.Rebind((StraightAttackAiBehaviorDefinition)def);
                    return straightAttackAiBehavior;
                case AiAttackType.Spray:
                    var sprayAttackAiBehavior = new SprayAttackAiBehavior();
                    sprayAttackAiBehavior.Rebind((SprayAttackAiBehaviorDefinition)def);
                    return sprayAttackAiBehavior;
                case AiAttackType.Radial:
                    var radialAttackAiBehavior = new RadialAttackAiBehavior();
                    radialAttackAiBehavior.Rebind((RadialAttackAiBehaviorDefinition)def);
                    return radialAttackAiBehavior;
                case AiAttackType.Sniper:
                    var sniperAttackAiBehavior = new SniperAttackAiBehavior();
                    sniperAttackAiBehavior.Rebind((SniperAttackAiBehaviorDefinition)def);
                    return sniperAttackAiBehavior;
                case AiAttackType.Thrust:
                    var thrustAttackAiBehavior = new ThrustAttackAiBehavior();
                    thrustAttackAiBehavior.Rebind((ThrustAttackAiBehaviorDefinition)def);
                    return thrustAttackAiBehavior;
                case AiAttackType.LaserSweep:
                    var laserSweepAttackAiBehavior = new LaserSweepAttackAiBehavior();
                    laserSweepAttackAiBehavior.Rebind((LaserSweepAttackAiBehaviorDefinition)def);
                    return laserSweepAttackAiBehavior;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IAiBehavior<IVehicle> GetBehavior(AiBehaviorDefinition def)
        {
            return GetBehavior(def as AttackAiBehaviorDefinition);
        }
    }
}