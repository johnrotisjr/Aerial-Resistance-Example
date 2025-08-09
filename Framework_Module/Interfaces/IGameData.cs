namespace Framework_Module.Interfaces
{
    public interface IGameData : IGameService
    {
        public IPersistentPlayerData PersistentPlayerData { get; }
        public ITransientPlayerData TransientPlayerData { get; }
    }
}
