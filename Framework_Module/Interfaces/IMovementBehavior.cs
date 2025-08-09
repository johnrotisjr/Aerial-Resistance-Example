using Framework_Module.Configs.Ai;
using Framework_Module.Interfaces;
using UnityEngine.PlayerLoop;

namespace Framework_Module.Interfaces
{
    /// <summary>
    /// Interface for movement-specific AI behaviors executed during a pattern.
    /// </summary>

    public interface IMovementBehavior : IBehavior
    {
        public int CompletedCycles { get; }
        public void Reset(AiMovementBehaviorConfig data);
    }
}