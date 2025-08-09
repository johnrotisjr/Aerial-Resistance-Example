using System.Collections.Generic;
using Framework_Module.Definitions;
using Framework_Module.Enums;

namespace Framework_Module.Interfaces
{
    public interface ITransientPlayerData
    {
        public void Clear();
        public VehicleArchetype? SelectedPlaneType { get; }
        public IReadOnlyList<WeaponDefinition> SelectedWeaponData { get; }
        public float FirePower { get; }
        public int Cash { get; }
        public int EnemiesDestroyed { get; }
        public int BossesDestroyed { get; }
        public int SelectedMissionIndex  { get; }
        public void SetMissionSelection(int index);
        public void AddCash(int cash);
        public void AddFirepower(float increment);
        public void SetVehicleSelection(VehicleArchetype selectedPlane);
        public void SetWeaponSelections(WeaponDefinition[] selectedWeapons);
    }
}
