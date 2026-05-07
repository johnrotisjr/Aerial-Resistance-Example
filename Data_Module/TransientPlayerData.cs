using System.Collections.Generic;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.Combat;
using Framework_Module.Interfaces;

namespace Data_Module
{
    public class TransientPlayerData : ITransientPlayerData
    {
        public string SelectedVehicleId => vehicleId;
        public IReadOnlyList<WeaponDefinition> SelectedWeaponData => weaponDataSelected;
        public float FirePower { get; private set; } = 1f;
        public int Cash { get; private set; }
        public int EnemiesDestroyed { get; private set; }
        public int BossesDestroyed { get; private set; }
        public int SelectedMissionIndex => missionIndex;
        
        private readonly EventBus eventBus;
        private int missionIndex = 0;
        private string vehicleId;
        private WeaponDefinition[] weaponDataSelected;

        public TransientPlayerData(EventBus eventBusService)
        {
            eventBus = eventBusService;
            eventBus.Subscribe<WorldObjectKilledEvent>(OnWorldObjectDestroyed);
            eventBus.Subscribe<BossDestroyedEvent>(OnBossDestroyed);
        }

        ~TransientPlayerData()
        {
            eventBus.Unsubscribe<WorldObjectKilledEvent>(OnWorldObjectDestroyed);
            eventBus.Unsubscribe<BossDestroyedEvent>(OnBossDestroyed);
        }

        public void SetMissionSelection(int index)
        {
            missionIndex = index;
        }
        
        public void AddCash(int cash)
        {
            Cash += cash;
        }
        
        public void AddFirepower(float increment)
        {
            FirePower += increment;
        }

        public void SetVehicleSelection(string id)
        {
            vehicleId = id;
        }
        
        public void SetWeaponSelections(WeaponDefinition[] selectedWeapons)
        {
            weaponDataSelected = selectedWeapons;
        }

        public void Clear()
        {
            missionIndex = 0;
            vehicleId = null;
            weaponDataSelected = null;
            BossesDestroyed = 0;
            EnemiesDestroyed = 0;
            FirePower = 1;
            Cash = 0;
        }
        
        private void OnBossDestroyed(BossDestroyedEvent e)
        {
            BossesDestroyed++;
        }
        
        private void OnWorldObjectDestroyed(WorldObjectKilledEvent e)
        {
            if (e.WorldObject is IVehicle vehicle && vehicle.AlignmentType == AlignmentType.Foe)
                EnemiesDestroyed++;
        }
    }
}