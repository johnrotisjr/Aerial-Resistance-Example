using Framework_Module.Event;
using UnityEngine;

namespace Framework_Module.Interfaces
{
    public interface IHud : IGameService
    {
        public void Inject(EventBus eventBusService, IPlayerController playerControllerService);
        public Vector2 GetHudPpuScaledSize();
    }
}