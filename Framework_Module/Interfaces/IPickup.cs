namespace Framework_Module.Interfaces
{
    public interface IPickup : IWorldObject
    {
        public float Value { get; }
        public void Apply();
    }
}