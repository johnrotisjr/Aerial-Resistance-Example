
using Framework_Module.Configs.Ai;

namespace Framework_Module.Interfaces
{
    /// <summary>
    /// Interface for attack-specific AI behaviors executed during a pattern.
    /// </summary>

    public interface IAttackBehavior : IBehavior
    {
        public void Reset(AiAttackBehaviorConfig data);
    }
}