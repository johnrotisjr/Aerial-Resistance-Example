using Framework_Module.Definitions;

namespace Framework_Module.Interfaces
{
    public interface IPickup : IWorldObject, IPooled<PickupDefinition>
    {
        public float Value { get; }
        public void Apply();
    }
}