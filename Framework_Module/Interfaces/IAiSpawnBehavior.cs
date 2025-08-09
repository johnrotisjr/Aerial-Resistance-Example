using Framework_Module.Definitions;
using Framework_Module.Interfaces;
using UnityEngine.PlayerLoop;

namespace Framework_Module.Interfaces
{
    /// <summary>
    /// Interface for spawn-specific AI behaviors executed during a pattern.
    /// </summary>

    public interface ISpawnMovementBehavior : IBehavior
    {
        public void Reset(SpawnMovementInstruction data);
    }
}