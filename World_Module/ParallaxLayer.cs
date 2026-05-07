using System.Collections.Generic;
using Framework_Module.Event;
using Framework_Module.Event.System;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;

namespace World_Module
{
    /// <summary>
    /// Parallax layer that tiles one sprite horizontally to fill the camera width.
    /// Scaling uses an integer pixel multiple so that the sprite remains pixel perfect.
    /// Layout and movement are computed in screen pixels to avoid subpixel seams.
    /// </summary>
    public class ParallaxLayer : MonoBehaviour
    {
        [SerializeField] private ParallaxTile tilePrefab;
        [SerializeField, Min(0f)] private float parallaxMultiplier = 0.5f;
        [SerializeField] private bool loop = true;
        [SerializeField] private ParallaxLayer scaleReference;
        [SerializeField] private bool scaleToView = false;
        [SerializeField] private Sprite tileSprite;
        [SerializeField] private int tileSpriteOrdering;
        [SerializeField] private float yOffset;

        private readonly List<ParallaxTile> tiles = new();
        // Authoritative X positions for each tile in integer pixels.
        // Transforms are derived from these pixel values each frame.
        private readonly List<int> tileXPx = new();

        private IViewportBoundsProvider viewportBoundsProvider;
        private EventBus eventBus;

        private int tileWidthPx;        // Exact width of one tile in pixels after scaling.
        private int yPx;                // Y position in pixels for this layer.
        //private int pixelScaleK = 1;    // Integer pixel scale factor (1x, 2x, 3x, ...).
        private float scaleFactor = 1f; // World-space scale applied to each tile transform.
        private float moveRemainderWu;  // Subpixel world-space remainder accumulated between pixel steps.

        public delegate void ScaledToView();
        public event ScaledToView OnScaledToView;

        // Pixels per world unit provided by the active view.
        // This is used to convert between pixel-space and world-space.
        private float Ppu => viewportBoundsProvider.Ppu;

        private void Awake()
        {
            // Resolve services used for viewport math and for listening to resolution changes.
            viewportBoundsProvider = Services.Get<IViewportBoundsProvider>();
            eventBus = Services.Get<EventBus>();
            eventBus.Subscribe<ResolutionChangedEvent>(OnResolutionChangedEvent);

            // If this layer follows another layer's scaling, subscribe to its scale event.
            if (scaleReference)
                scaleReference.OnScaledToView += OnScaledToViewEvent;
        }

        private void Start()
        {
            // Build the initial strip of tiles sized and positioned to fill the view.
            CreateTiles();
        }

        private void OnDestroy()
        {
            // Detach event handlers to avoid leaks or dangling callbacks.
            if (scaleReference)
                scaleReference.OnScaledToView -= OnScaledToViewEvent;

            if (eventBus != null)
                eventBus.Unsubscribe<ResolutionChangedEvent>(OnResolutionChangedEvent);
        }

        // ---------- Pixel helpers ----------

        // Convert integer pixels to world units using the current PPU.
        private float PixelsToWorldUnits(int px) => px / Ppu;

        // Convert world units to integer pixels using the current PPU with rounding to the nearest pixel.
        private int WorldUnitsToPixels(float wu)  => Mathf.RoundToInt(wu * Ppu);

        /// <summary>
        /// Computes wo values from the current sprite and view:
        /// 1) scaleFactor: world-space transform scale that achieves pixelScaleK at the current PPU
        /// 2) tileWidthPx: exact tile width in pixels after applying pixelScaleK
        /// If no sprite is set, resets to neutral values.
        /// </summary>
        private void RecomputeScaleAndTileWidthPx()
        {
            var sprite = tileSprite;
            if (!sprite)
            {
                //pixelScaleK = 1;      // keep for compatibility
                tileWidthPx  = 0;
                scaleFactor  = 1f;
                return;
            }

            // View height in pixels, aligned to the camera grid
            int viewPxH = Mathf.Max(1, Mathf.RoundToInt(viewportBoundsProvider.GetPixelAlignedViewport().height * Ppu));

            // Sprite source size in pixels
            int spritePxH = Mathf.RoundToInt(sprite.rect.height);
            int spritePxW = Mathf.RoundToInt(sprite.rect.width);

            // Choose integer upscale (p) or integer downscale divisor (q)
            int p = 1; // upscale multiple
            int q = 1; // downscale divisor

            if (viewPxH >= spritePxH)
            {
                var multiple = Mathf.FloorToInt((float)viewPxH / spritePxH) + ((viewPxH % spritePxH > 0) ? 1 : 0);
                // Upscale: largest integer multiple that still fits
                p = Mathf.Max(1, multiple);
                q = 1;
            }
            else
            {
                var multiple = Mathf.CeilToInt((float)spritePxH / viewPxH) + ((spritePxH % viewPxH > 0) ? 1 : 0);
                // Downscale: smallest integer divisor that makes it fit
                // Requires sprite dimensions divisible by q for exact pixel-perfect minification
                q = Mathf.Max(1, multiple);
                p = 1;
            }

            // Legacy field kept: treat "pixelScaleK" as net numerator p, so K = p/q for reference
            //pixelScaleK = p;

            // World scale so that 1 source pixel maps to p/q screen pixels
            // sprite.pixelsPerUnit converts sprite pixels to world units
            // Ppu converts world units to screen pixels
            // Net: (p/q) * (spritePPU / cameraPPU)
            scaleFactor = (p / (float)q) * (sprite.pixelsPerUnit / Ppu);

            // Tile width in on-screen pixels after scaling. Keep integer.
            tileWidthPx = Mathf.RoundToInt(spritePxW * (p / (float)q));
        }


        /// <summary>
        /// Ensures the horizontal strip contains enough tiles to fully cover the current view width.
        /// Creates more tiles if needed. Destroys extras if the view got smaller.
        /// The "+ 1" buffer prevents a gap from appearing when scrolling.
        /// </summary>
        private void EnsureTileCoverageForWidth()
        {
            if (tileWidthPx <= 0) return;

            // View width in pixels so that required tile count can be computed in integer space.
            int viewWidthPx = Mathf.RoundToInt(viewportBoundsProvider.GetPixelAlignedViewport().width * Ppu);

            // Number of tiles needed so that the rightmost tile extends beyond the right edge.
            // Ceiling division ensures full coverage. The extra one is a safety buffer.
            int needed = Mathf.CeilToInt(viewWidthPx / (float)tileWidthPx) + 1;

            // If there are too few tiles, instantiate and append until the count matches.
            if (needed > tiles.Count)
            {
                for (int i = tiles.Count; i < needed; i++)
                    tiles.Add(CreateNewTile());
            }
            // If there are too many tiles, remove the extras from the end.
            else if (needed < tiles.Count)
            {
                for (int i = tiles.Count - 1; i >= needed; i--)
                {
                    Destroy(tiles[i].gameObject);
                    tiles.RemoveAt(i);
                }
            }
        }

        // ---------- Creation & scaling ----------

        /// <summary>
        /// Constructs the initial set of tiles and positions them from the left camera edge.
        /// If scaleToView is true then per-tile scale is computed from the sprite and view.
        /// Otherwise scale is inherited from a reference or left at prefab scale.
        /// Pixel-perfect Y is computed from yOffset.
        /// </summary>
        private void CreateTiles()
        {
            // Start lists so that layout starts from a clean state.
            tiles.Clear();
            tileXPx.Clear();

            RecomputeScaleAndTileWidthPx();

            // Create the first tile. Its scale is set inside CreateNewTile.
            var first = CreateNewTile();
            tiles.Add(first);

            // Ensure enough tiles exist to fill the view horizontally.
            EnsureTileCoverageForWidth();

            // Compute the layer Y position in integer pixels from the provided world-space offset.
            yPx = Mathf.RoundToInt(yOffset * Ppu);

            // Compute integer X positions for each tile starting at the camera left edge.
            AlignTilesPx();

            // Push the computed pixel positions into the actual world transforms.
            FlushTransformsFromPixels();
        }

        /// <summary>
        /// Instantiates a tile, applies the sprite and render order, and sets its scale.
        /// If scaleToView is true, applies the computed scaleFactor for an integer pixel scale.
        /// If a scaleReference is present and scaleToView is false, copies its scaleFactor.
        /// </summary>
        private ParallaxTile CreateNewTile()
        {
            // Instantiate under this layer so the tiles move with the layer.
            var tile = Instantiate(tilePrefab, transform);

            // Assign visual data.
            tile.SetSprite(tileSprite);
            tile.SetSpriteOrdering(tileSpriteOrdering);

            if (scaleToView)
            {
                // Recompute before applying in case this method is called at a different time than CreateTiles.
                RecomputeScaleAndTileWidthPx();
                tile.Scale(scaleFactor);
            }
            else if (scaleReference)
            {
                // Mirror the reference layer's scale so multiple layers line up in pixels.
                tile.Scale(scaleReference.scaleFactor);
            }

            return tile;
        }

        /// <summary>
        /// Handles screen or resolution changes.
        /// Recomputes scale to keep the sprite height fitting the new view height.
        /// Rescales existing tiles, adjusts tile count for the new width, and realigns positions.
        /// Emits OnScaledToView after transforms are updated.
        /// </summary>
        private void OnResolutionChangedEvent(ResolutionChangedEvent _)
        {
            // Only act when this layer owns its scaling.
            if (!scaleToView) return;

            // Recompute the integer pixel scale and dependent values for the new view.
            RecomputeScaleAndTileWidthPx();

            // Apply the new world-space scale to all existing tiles.
            foreach (var t in tiles)
                t.Scale(scaleFactor);

            // Add or remove tiles as the horizontal coverage requirement changed.
            EnsureTileCoverageForWidth();

            // Recompute Y offset in pixel space and realign all tiles in pixel X and Y.
            yPx = Mathf.RoundToInt(yOffset * Ppu);
            AlignTilesPx();
            FlushTransformsFromPixels();

            // Notify listeners that this layer has finished applying the new scale.
            OnScaledToView?.Invoke();
        }

        /// <summary>
        /// Reacts when a referenced layer reports that it has updated its scale.
        /// Copies scale from the reference, recomputes our pixel width from world size,
        /// ensures coverage, and realigns positions in pixel space.
        /// </summary>
        private void OnScaledToViewEvent()
        {
            // Copy the world-space scale so this layer stays pixel aligned with the reference.
            foreach (var t in tiles)
                t.Scale(scaleReference.scaleFactor);

            // Convert the first tile's world width back to integer pixels for layout.
            // This keeps tileWidthPx consistent with the applied scale.
            tileWidthPx = tiles.Count > 0 ? WorldUnitsToPixels(tiles[0].ActualSizeInWorldUnits().x) : tileWidthPx;

            // Make sure the horizontal strip still covers the view at the new size.
            EnsureTileCoverageForWidth();

            // Update Y in pixels and realign X positions, then apply to transforms.
            yPx = Mathf.RoundToInt(yOffset * Ppu);
            AlignTilesPx();
            FlushTransformsFromPixels();
        }

        // ---------- Layout in pixels ----------

        /// <summary>
        /// Computes the integer pixel X position for each tile so that the strip starts
        /// at the camera's left edge and continues right with no gaps or overlaps.
        /// </summary>
        private void AlignTilesPx()
        {
            // Get the camera-aligned left world coordinate, convert to integer pixels.
            int camLeftPx = Mathf.RoundToInt(viewportBoundsProvider.GetPixelAlignedViewport().xMin * Ppu);

            tileXPx.Clear();
            // Place each tile directly after the previous one using exact pixel width.
            for (int i = 0; i < tiles.Count; i++)
                tileXPx.Add(camLeftPx + i * tileWidthPx);
        }

        /// <summary>
        /// Writes the authoritative integer pixel positions into each tile's Transform.
        /// Converts pixels back to world units using PPU. Z is preserved.
        /// </summary>
        private void FlushTransformsFromPixels()
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                var t = tiles[i];
                t.Position = new Vector3(PixelsToWorldUnits(tileXPx[i]), PixelsToWorldUnits(yPx), t.Position.z);
            }
        }

        // ---------- Scrolling ----------

        /// <summary>
        /// Scrolls the tiled background at a fraction of the global speed.
        /// Movement is accumulated in world units then quantized to whole pixels to avoid subpixel drift.
        /// If looping is enabled, tiles that move fully off the left edge are recycled to the right.
        /// </summary>
        public void Scroll(float globalSpeed, float deltaTime)
        {
            if (tileWidthPx <= 0 || tiles.Count == 0) return;

            // Compute world-space displacement for this frame using the parallax ratio.
            float movementWu = globalSpeed * parallaxMultiplier * deltaTime;

            // Accumulate subpixel world-space motion. This allows consistent pixel stepping over time.
            moveRemainderWu += movementWu;

            // Convert the accumulated motion to whole pixels. Only full pixels are applied to layout.
            int pixelStep = Mathf.FloorToInt(moveRemainderWu * Ppu);
            if (pixelStep > 0)
            {
                // Apply the pixel shift to all authoritative X positions.
                for (int i = 0; i < tileXPx.Count; i++)
                    tileXPx[i] -= pixelStep;

                // Remove exactly the pixels that were applied, converted back to world units.
                moveRemainderWu -= pixelStep / Ppu;

                // If looping is active, move fully offscreen tiles from the left to the right end.
                if (loop)
                    RecycleTilesIfNeededPx();
            }

            // Reflect the pixel-space positions into the tile transforms.
            FlushTransformsFromPixels();
        }

        // ---------- Recycling in pixels ----------

        /// <summary>
        /// Recycles tiles that have fully moved off the left side of the camera view.
        /// The tile with the smallest X is moved to exactly the right edge of the rightmost tile.
        /// Uses only integer pixel math to preserve pixel perfect alignment.
        /// </summary>
        private void RecycleTilesIfNeededPx()
        {
            int camLeftPx = Mathf.RoundToInt(viewportBoundsProvider.GetPixelAlignedViewport().xMin * Ppu);
            if (tileWidthPx <= 0 || tiles.Count == 0) return;

            // Safety guard avoids infinite loops if data becomes inconsistent.
            int safety = 0;
            while (safety++ < tiles.Count + 2)
            {
                // Find indices of the leftmost and rightmost tiles by pixel X.
                int leftIdx = 0, rightIdx = 0;
                int leftX = tileXPx[0], rightX = tileXPx[0];

                for (int i = 1; i < tileXPx.Count; i++)
                {
                    int x = tileXPx[i];
                    if (x < leftX) { leftX = x; leftIdx = i; }
                    if (x > rightX) { rightX = x; rightIdx = i; }
                }

                // If the leftmost tile's right edge is at or left of the camera left edge,
                // it is fully offscreen. Move it to the immediate right of the rightmost tile.
                if (leftX + tileWidthPx <= camLeftPx)
                {
                    tileXPx[leftIdx] = rightX + tileWidthPx;
                    continue;
                }

                // No more tiles need recycling for this frame.
                break;
            }
        }
    }
}
