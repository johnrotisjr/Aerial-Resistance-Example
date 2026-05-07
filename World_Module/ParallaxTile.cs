using UnityEngine;
using World_Module.WorldObjects;

namespace World_Module
{
    internal class ParallaxTile : WorldObject
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        public void SetSpriteOrdering(int spriteSortingOrder)
        {
            spriteRenderer.sortingOrder = spriteSortingOrder;
        }
        
        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }
    }
}