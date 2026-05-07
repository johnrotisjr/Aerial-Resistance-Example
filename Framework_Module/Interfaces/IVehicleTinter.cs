using UnityEngine;

namespace Framework_Module.Interfaces
{
    public interface IVehicleTinter
    {
        public void Tint(Color color);
        public void ResetTint();
        public void TintOverTime(Color color);
    }
}