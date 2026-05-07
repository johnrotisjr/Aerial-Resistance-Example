using Debug_Module;
using Framework_Module.Extensions;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;

namespace World_Module.WorldObjects
{
    /// <summary>
    /// Base MonoBehaviour for all gameplay entities that can move, collide, and render in the world.
    /// Provides common behavior and references such as Rigidbody and SpriteRenderer.
    /// </summary>

    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    internal abstract class WorldObject : MonoBehaviour, IWorldObject
    {
        protected SpriteRendererBoundsAggregator SpriteRendererBoundsAggregator;
        protected Rigidbody2D Rigidbody;
        protected Collider2D Collider;
        private IViewportBoundsProvider viewportBoundsProvider;
        private CollisionResolver collisionResolver;
        private string guid;
        public string Guid => guid;
        public Vector3 LocalScale => transform.localScale;
        public Transform Transform => transform;
        public bool IsColliderEnabled => Collider.enabled;

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public GameObject GameObject => gameObject;

        public Vector3 Position
        {
            get => transform.position;
            set => SetPosition(value);
        }

        public Quaternion WorldRotation => transform.rotation;

        public void AddPosition(Vector3 increment)
        {
            SetPosition(Position + increment);
        }
        
        public Bounds SpriteBounds => SpriteRendererBoundsAggregator.GetBounds();
        public Rect SpriteBoundsRect => SpriteRendererBoundsAggregator.GetBounds().BoundsToRect();

        public bool IsFacingRight => Transform.localScale.x > 0;
        
        public Vector2 Velocity => Rigidbody.linearVelocity;
        
        public virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<Collider2D>();
            SpriteRendererBoundsAggregator = GetComponent<SpriteRendererBoundsAggregator>();
            collisionResolver = Services.Get<CollisionResolver>();
            viewportBoundsProvider = Services.Get<IViewportBoundsProvider>();
            guid = System.Guid.NewGuid().ToString();
        }
        
        private void SetPosition(Vector3 position)
        {
            position = viewportBoundsProvider.SnapToPixelAlignedPosition(position);
            transform.position = position;
        }

        public Vector3 ActualSizeInWorldUnits()
        {
            return SpriteRendererBoundsAggregator.GetBounds().size;
        }
        
        public void Scale(float scaleFactor)
        {
            transform.localScale = Vector3.one * scaleFactor;
        }

        public void Scale(float x, float y, float z)
        {
            transform.localScale = new Vector3(x, y, z);
        }

        public virtual void SetVelocity(Vector2 velocity)
        {
            Rigidbody.linearVelocity = velocity;
        }
        
        public void EnableCollider(bool isEnabled)
        {
            Collider.enabled = isEnabled;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Collider.TryGetComponent<WorldObject>(out var worldObjectA) && other.TryGetComponent<WorldObject>(out var worldObjectB))
            {
                collisionResolver.HandleCollision(worldObjectA, worldObjectB);
            }
            else
            {
                DebugLogger.Log("Collision with none WorldObject type detected!", LogCategory.World, LogLevel.Error);
            }
 
        }
    }
}