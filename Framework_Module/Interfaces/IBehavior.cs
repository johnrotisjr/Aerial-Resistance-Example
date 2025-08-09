namespace Framework_Module.Interfaces
{
    /// <summary>
    /// Interface for AI behaviors (movement or attack) that can be executed by a Vehicle.
    /// </summary>

    public interface IBehavior
    {
        public void Execute(IWorldObject worldObject);
        public void EndBehavior(IWorldObject worldObject);
    }
}