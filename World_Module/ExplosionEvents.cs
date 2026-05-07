using UnityEngine;
using World_Module.WorldObjects;

namespace World_Module
{
    public class ExplosionEvents : MonoBehaviour
    {
        [SerializeField] private Vehicle vehicle;
        public void OnExplosionComplete()
        {
            vehicle.OnExplosionComplete();
        }
    }
}
