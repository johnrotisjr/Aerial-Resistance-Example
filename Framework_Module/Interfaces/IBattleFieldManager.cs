using UnityEngine;

namespace Framework_Module.Interfaces
{
    
    public interface IBattleFieldManager : IGameService
    {
        public void Inject(IViewportBoundsProvider viewportBoundsProviderService, IWorldStateManager worldStateManagerService);
    }
}
