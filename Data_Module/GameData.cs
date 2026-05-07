using Framework_Module.Event;
using Framework_Module.Interfaces;

namespace Data_Module
{
    public class GameData : IGameData
    {
        public IPersistentPlayerData PersistentPlayerData { get; private set; }
        public ITransientPlayerData TransientPlayerData { get; private set; }

        public GameData(IPersistentPlayerData persistentData, ITransientPlayerData transientData)
        {
            PersistentPlayerData = persistentData;
            TransientPlayerData = transientData;
        }
        public void Initialize()
        {
            
        }

        public void Shutdown()
        {
            
        }
        
        
    }
}