using Framework_Module.Definitions;

namespace Framework_Module.Interfaces
{
    /// <summary>
    /// Interface for AI behaviors that can be executed by a Vehicle.
    /// </summary>
    
    public interface IAiBehavior<in TAgent> where TAgent : IWorldObject 
    {
        public void Start(TAgent agent);
        public void Tick(float deltaTime, TAgent agent);
        public void End(TAgent worldObject);
    }
}