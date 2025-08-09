using Framework_Module.Definitions;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Interfaces
{
    public interface IPlayerController : IVehicleController, IGameService
    {
        public void Move(Vector2 direction);
        public float Speed { get; }
        public void AddPower(float powerIncrease);
        public WeaponDefinition? CurrentlyEquippedWeapon();
        public void ProcessInput(DigitalInputActionType type, bool isPressed);
        public void ProcessInput(AxisInputActionType type, Vector2 inputValue);
    }
}