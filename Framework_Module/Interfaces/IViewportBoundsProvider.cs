using UnityEngine;

namespace Framework_Module.Interfaces
{
    public interface IViewportBoundsProvider : IGameService
    {
        public bool IsOutOfView(Bounds bounds);
        public Vector3 SnapToPixelAlignedPosition(Vector3 raw);
        public Rect GetPixelAlignedViewport(float buffer = 0);
        public Rect GetPixelAlignedViewport(Vector2 buffer);
        public Rect GetBattleFieldRect();
        public Bounds WorldViewBounds { get; }
        public int Ppu { get; }

    }
}
