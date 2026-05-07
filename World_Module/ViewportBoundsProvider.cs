using Framework_Module.Event;
using Framework_Module.Event.System;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;
using UnityEngine.U2D;

namespace World_Module
{
    /// <summary>
    /// Manages camera bounds and viewport dimensions.
    /// Provides clamping methods for World Object positions and utility methods for viewport dimensions.
    /// </summary>

    [RequireComponent(typeof(Camera))]
    public class ViewportBoundsProvider : GameServiceBase, IViewportBoundsProvider
    {
        private Camera gameCamera;
        private PixelPerfectCamera pixelPerfectCamera;
        private EventBus eventBus;
        private const float BattleFieldBuffer = 10f;
        private const float DeathBoundaryBuffer = 25;
        
        public Bounds WorldViewBounds { get; private set; }
        public int Ppu => pixelPerfectCamera != null ? pixelPerfectCamera.assetsPPU : 32;
        private bool isInitialized = false;
        private Vector2 HudSize => Services.Get<IHud>()?.GetHudPpuScaledSize() ?? Vector2.zero;

        public override void Initialize()
        {
            if (isInitialized)
                return;
            
            gameCamera = GetComponent<Camera>();
            pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
            ResetWorldViewBounds();
            eventBus = Services.Get<EventBus>();
            eventBus?.Subscribe<ResolutionChangedEvent>(OnResolutionChanged);
            isInitialized = true;
        }

        private void ResetWorldViewBounds()
        {
            float planeDist = Mathf.Abs(gameCamera.transform.position.z);
            // bottom-left corner of the window in world‐units
            Vector3 bl = gameCamera.ScreenToWorldPoint(new Vector3(0, 0, planeDist));
            // top-right corner of the window in world‐units
            Vector3 tr = gameCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, planeDist));
            // build your Bounds from those two points
            Vector2 size = new Vector2(tr.x - bl.x, tr.y - bl.y);
            Vector3 center = (bl + tr) * 0.5f;
            WorldViewBounds = new Bounds(center, size);
        }

        private void OnResolutionChanged(ResolutionChangedEvent e)
        {
            ResetWorldViewBounds();
        }

        public bool IsOutOfView(Bounds bounds)
        {
            return bounds.min.x < WorldViewBounds.min.x || bounds.max.x > WorldViewBounds.max.x || bounds.min.y < WorldViewBounds.min.y || bounds.max.y > WorldViewBounds.max.y;
        }

        public Vector3 SnapToPixelAlignedPosition(Vector3 raw)
        {
            raw.x = Mathf.Round(raw.x * Ppu) / Ppu;
            raw.y = Mathf.Round(raw.y * Ppu) / Ppu;
            return raw;
        }

        private Rect GetViewportRect(float buffer = 0, bool excludeHudSafeAreaHeight = false)
        {
            return GetViewportRect(new Vector2(buffer, buffer), excludeHudSafeAreaHeight);
        }
        
        private Rect GetViewportRect(Vector2 buffer, bool excludeHudSafeAreaHeight = false)
        {
            float left = WorldViewBounds.min.x - buffer.x;
            float right = WorldViewBounds.max.x + buffer.x;
            float top = WorldViewBounds.max.y + buffer.y + (excludeHudSafeAreaHeight ? HudSize.y : 0);
            float bottom = WorldViewBounds.min.y - buffer.y;

            return new Rect(
                left,
                bottom,
                right - left,
                top - bottom
            );
        }
 
        public Rect GetBattleFieldRect()
        {
            return GetViewportRect(BattleFieldBuffer);
        }
        
        public Rect GetDeathBoundaryRect()
        {
            return GetViewportRect(DeathBoundaryBuffer);
        }
        
        private float FloorToPpu(float v) => Mathf.Floor(v * Ppu) / Ppu;
        private float CeilToPpu (float v) => Mathf.Ceil (v * Ppu) / Ppu;
        public Rect GetPixelAlignedViewport(float buffer = 0f, bool excludeUiSafeArea = false)
        {
            return GetPixelAlignedViewport(new Vector2(buffer,buffer), excludeUiSafeArea);
        }
        
        public Rect GetPixelAlignedViewport(Vector2 buffer, bool excludeHudSafeAreaHeight = false)
        {
            // 1) get the raw (unsnapped) viewport in world units
            var raw= GetViewportRect(buffer, excludeHudSafeAreaHeight);

            // 2) snap the min edges down (so we never under-draw)
            float left = FloorToPpu(raw.xMin);
            float bottom = FloorToPpu(raw.yMin);

            // 3) snap the max edges up (so we cover every pixel)
            float right = CeilToPpu(raw.xMax);
            float top = CeilToPpu(raw.yMax);

            // 4) rebuild a Rect from those snapped edges
            return new Rect(
                left,
                bottom,
                right-left,
                top-bottom
            );
        }

        public override void Shutdown()
        {
            eventBus.Unsubscribe<ResolutionChangedEvent>(OnResolutionChanged);
        }
    }
}
