using UnityEngine;

namespace Framework_Module.Interfaces
{
    public interface IWorldObject
    {
        public Vector3 ActualSizeInWorldUnits();
        public void AddPosition(Vector3 increment);
        public void Scale(float scaleFactor);
        public void SetSprite(Sprite sprite);
        public void SetVelocity(Vector2 velocity);
        public void SetRotation(Quaternion rotation);
        public Vector3 Position { get; set; }
        public Bounds SpriteBounds { get; }
        public Rect SpriteBoundsRect { get;  }
        public bool IsFacingRight { get; }
        public Vector2 Velocity { get; }
        public Transform Transform { get; }
    }
}