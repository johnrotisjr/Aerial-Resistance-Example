using Framework_Module.Event;
using Framework_Module.Event.Gameplay;
using Framework_Module.Event.Ui;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;

namespace Ui_Module.HudObjects
{
    /// <summary>
    /// Manages HUD elements and dispatches update calls in response to game events.
    /// Acts as a container for all active HUD components.
    /// </summary>
    public class Hud : MonoBehaviour, IHud
    {
        [SerializeField] private RectTransform hudRectTransform;
        private HudElement[] hudElements;
        private EventBus eventBus;
        private int ppu;

        public void Inject(EventBus eventBusService, IPlayerController playerControllerService)
        {
            eventBus = eventBusService;
            hudElements = GetComponentsInChildren<HudElement>();
            foreach (var element in hudElements)
            {
                element.Inject(eventBusService, playerControllerService);
            }
        }

        public Vector2 GetHudPpuScaledSize()
        {
            var rect = hudRectTransform.rect;
            return new Vector2(rect.x / ppu, rect.y / ppu);
        }

        private void ResetHud()
        {
            foreach (var hudElement in hudElements)
            {
                hudElement.ResetHud();
            }
        }

        private void OnRestartMission(RestartMissionEvent e)
        {
            ResetHud();
        }
        
        private void OnUpdateHud(UpdateHudEvent e)
        {
            foreach (var hudElement in hudElements)
            {
                hudElement.UpdateHud();
            }
        }
        
        public void Initialize()
        {
            ppu = Services.Get<IViewportBoundsProvider>().Ppu;
            eventBus.Subscribe<UpdateHudEvent>(OnUpdateHud);
            eventBus.Subscribe<RestartMissionEvent>(OnRestartMission);
        }

        public void Shutdown()
        {
            eventBus.Unsubscribe<RestartMissionEvent>(OnRestartMission);
            eventBus.Unsubscribe<UpdateHudEvent>(OnUpdateHud);
        }
    }
}
