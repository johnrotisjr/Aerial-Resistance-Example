using Framework_Module.Definitions;

namespace Framework_Module.Interfaces
{
    /// <summary>
    /// Interface for movement-specific AI behaviors executed during a pattern.
    /// </summary>
    
    public interface IMovementBehavior : IAiBehavior<IVehicle>
    {
        public delegate void CompletedMovementCycle();
        public event CompletedMovementCycle OnCompletedMovementCycle;
    }
}