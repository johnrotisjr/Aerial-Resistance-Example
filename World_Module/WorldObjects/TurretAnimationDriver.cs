using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;

namespace World_Module.WorldObjects
{
    //TODO: REVIEW
    public class TurretAnimationDriver : MonoBehaviour
    {
        private IPlayerController playerController;
        private Animator animator;
        private readonly int playerAngleHash = Animator.StringToHash("playerAngle");

        public void Awake()
        {
            playerController = Services.Get<IPlayerController>();
            animator = GetComponent<Animator>();
        }
        
        public void LateUpdate()
        {
            if (playerController?.ControlledVehicle == null)
                return;

            var dir = playerController.ControlledVehicle.Position - transform.position;
            float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (ang < 0)
                ang = -ang;
            ang = Mathf.Clamp(ang, 0, 180);
            animator.SetFloat(playerAngleHash, ang);
        }

    }
}