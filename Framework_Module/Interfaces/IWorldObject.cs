using UnityEngine;

namespace Framework_Module.Interfaces
{
    public interface IWorldObject
    {
        public string Guid { get; }
        public Vector3 ActualSizeInWorldUnits();
        public void AddPosition(Vector3 increment);
        public void Scale(float scaleFactor);
        public void Scale(float x, float y, float z);
        public void SetVelocity(Vector2 velocity);
        public void SetRotation(Quaternion rotation);
        public Vector3 Position { get; set; }
        public Quaternion WorldRotation { get; }
        public Bounds SpriteBounds { get; }
        public Rect SpriteBoundsRect { get;  }
        public bool IsFacingRight { get; }
        public Vector2 Velocity { get; }
        public Vector3 LocalScale { get; }
        public Transform Transform { get; }
        public void EnableCollider(bool isEnabled);
        public bool IsColliderEnabled { get; }
        public GameObject GameObject { get; }
    }
}