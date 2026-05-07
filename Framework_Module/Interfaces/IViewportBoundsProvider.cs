using UnityEngine;

namespace Framework_Module.Interfaces
{
    public interface IViewportBoundsProvider : IGameService
    {
        public bool IsOutOfView(Bounds bounds);
        public Vector3 SnapToPixelAlignedPosition(Vector3 raw);
        public Rect GetPixelAlignedViewport(Vector2 buffer, bool excludeHudSafeAreaHeight = false);
        public Rect GetPixelAlignedViewport(float buffer = 0f, bool excludeHudSafeAreaHeight = false);
        public Rect GetBattleFieldRect();
        public Rect GetDeathBoundaryRect();
        public Bounds WorldViewBounds { get; }
        public int Ppu { get; }

    }
}
