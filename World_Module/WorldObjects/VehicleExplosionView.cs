using Framework_Module.Interfaces;
using UnityEngine;

namespace World_Module.WorldObjects
{
    [RequireComponent(typeof(Animator))]
    public class VehicleExplosionView : MonoBehaviour, IVehicleExplosionView
    {
        [SerializeField] private SpriteRenderer[] normalSpriteRenderers;
         
        public void ShowExplosion()
        {
            foreach (var sr in normalSpriteRenderers)
            {
                sr.enabled = false;
            }
            gameObject.SetActive(true);
 
        }

        public void ResetView()
        {
            foreach (var sr in normalSpriteRenderers)
            {
                sr.enabled = true;
            }
            gameObject.SetActive(false);
        }
    }
}