using Framework_Module.Event;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Ui_Module.HudObjects
{
    /// <summary>
    /// Abstract base class for HUD elements that support refresh and reset operations.
    /// Used for consistent HUD update architecture.
    /// </summary>
    internal abstract class HudElement : MonoBehaviour
    {
        protected EventBus EventBus;
        protected IPlayerController PlayerController;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public SpriteRenderer SpriteRenderer => spriteRenderer;
        public abstract void ResetHud();
        public abstract void UpdateHud();

        public void Inject(EventBus eventBusService, IPlayerController playerControllerService)
        {
            EventBus = eventBusService;
            PlayerController = playerControllerService;
        }
    }
}