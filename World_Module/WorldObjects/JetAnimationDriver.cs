using Framework_Module.Interfaces;
using UnityEngine;

namespace World_Module.WorldObjects
{

    [RequireComponent(typeof(Vehicle))]
    public class JetAnimationDriver : MonoBehaviour
    {
        private enum JetDirection
        {
            Down = -1,
            Idle = 0,
            Up = 1
        }

        private Animator animator;
        private IVehicle vehicle;
        private readonly int yDirectionAnimationHash = Animator.StringToHash("yDirection");

        public void Awake()
        {
            vehicle = GetComponent<Vehicle>();
            animator = GetComponent<Animator>();
        }

        public void LateUpdate()
        {
            JetDirection jetDirection = JetDirection.Idle;
            var velocityY = vehicle.Velocity.y;
            if (velocityY > 0.5)
            {
                jetDirection = JetDirection.Up;
            }
            else if (velocityY < -0.5)
            {
                jetDirection = JetDirection.Down;
            }
            
            animator.SetInteger(yDirectionAnimationHash, (int)jetDirection);
        }
    }
}