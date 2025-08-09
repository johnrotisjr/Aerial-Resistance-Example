using Framework_Module.Configs.Ai;
using Framework_Module.Interfaces;

namespace Ai_Module.Behaviors.Attack.Behavior
{
    /// <summary>
    /// AI attack behavior that performs no action.
    /// Used as a default or placeholder when no attack pattern is assigned.
    /// </summary>

    public class NoAttackBehavior : IAttackBehavior
    {
        public NoAttackBehavior()
        {
        }

        public void Execute(IWorldObject worldObject)
        {

        }

        public void EndBehavior(IWorldObject worldObject)
        {
            
        }

        public void Reset(AiAttackBehaviorConfig data)
        {
        }
    }
}