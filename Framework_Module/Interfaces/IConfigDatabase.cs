 
using Framework_Module.Definitions;
using Framework_Module.Enums;
using Framework_Module.GameData.Data;
using UnityEngine;

namespace Framework_Module.Interfaces
{
    /// <summary>
    /// Loads and provides access to all game configuration ScriptableObjects used throughout the game.
    /// Acts as a centralized config loader service.
    /// </summary>

    public interface IConfigDatabase : IGameService
    {
        public WeaponDefinition[] GetAllWeaponData();
        public VehicleData[] GetAllVehicleDefinition();
        public bool GetVehicleData(string id, out VehicleData data);
        public MissionDefinition GetMissionDefinition(int index);
        public bool GetWeaponDefinition(WeaponType type, out WeaponDefinition definition);
        public bool GetPickupDefinition(PickupType type, out PickupDefinition definition);
        public int GetRewardAmount(int missionIndex, RewardType rewardType);
        public GameObject PickupPrefab { get; }
        public bool GetMusicDefinition(AudioMusicType type, out AudioMusicDefinition definition);
        public bool GetSfxDefinition(AudioSfxType type, out AudioSfxDefinition definition);
        public LevelDefinition[] GetAllLevelDefinitions();
        public Sprite GetAvatarSprite(AvatarType type);
        public bool GetLevelDefinition(int levelIndex, out LevelDefinition definition);
        public bool GetUpgradeDefinition(UpgradeType type, out UpgradeDefinition definition);
        public DialogTrackDefinition? GetDialogSequenceDefinition(int missionIndex, WorldStateType type);
    }
}
