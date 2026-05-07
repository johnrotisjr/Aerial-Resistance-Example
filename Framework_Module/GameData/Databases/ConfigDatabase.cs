using System.Collections.Generic;
using System.Linq;
using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using Framework_Module.GameData.Configuration;
using Framework_Module.GameData.Data;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework_Module.GameData.Databases
{
    /// <summary>
    /// Loads and provides access to all game configuration ScriptableObjects used throughout the game.
    /// Acts as a centralized config loader service.
    /// </summary>
    //TODO: Split this class into several smaller classes
    public class ConfigDatabase : GameServiceBase, IConfigDatabase
    {
        [SerializeField] private VehicleDatabase vehicleDatabase;
        [SerializeField] private MissionDatabase missionDatabase;
        [SerializeField] private WeaponDatabase weaponDatabase;
        [SerializeField] private PickupDatabase pickupDatabase;
        [SerializeField] private RewardConfig rewardConfig;
        [SerializeField] private AudioDatabase audioDatabase;
        [FormerlySerializedAs("levelDatabase")] [SerializeField] private MissionRequirementsDatabase missionRequirementsDatabase;
        [SerializeField] private UpgradeConfig upgradeConfig;
        [SerializeField] private DialogCueDatabase dialogCueDatabase;
        [SerializeField] private AvatarDatabase avatarDatabase;
        
        public GameObject PickupPrefab => pickupDatabase.PickupPrefab;
        
        private readonly Dictionary<WeaponType, WeaponDefinition> weaponLookup = new();
        private readonly Dictionary<string, VehicleData> vehicleLookup = new();
        private readonly Dictionary<PickupType, PickupDefinition> pickupLookup = new();
        private readonly Dictionary<RewardType, RewardDefinition> rewardLookup = new();
        private readonly Dictionary<AudioMusicType, AudioMusicDefinition> musicLookup = new();
        private readonly Dictionary<AudioSfxType, AudioSfxDefinition> sfxLookup = new();
        private readonly Dictionary<int, LevelDefinition> levelLookup = new();
        private readonly Dictionary<UpgradeType, UpgradeDefinition> upgradeLookup = new();
        private readonly Dictionary<int, DialogCue> dialogCueLookup = new();
        private readonly Dictionary<AvatarType, AvatarDefinition> avatarLookup = new();
        
#if UNITY_EDITOR
        public bool TryGetVehicleCooldown(string id, out VehicleData vehicleData)
        {
            foreach (var data in vehicleDatabase.Database)
            {
                if (data.Guid == id)
                {
                    vehicleData = data;
                    return true;
                }
            }

            vehicleData = null;
            return false;
        }
#endif
        
        public bool GetMusicDefinition(AudioMusicType type, out AudioMusicDefinition definition)
        {
            if (musicLookup.TryGetValue(type, out definition))
                return true;
    
            DebugLogger.Log($"Music type {type} not found!", LogCategory.Framework, LogLevel.Error);
            return false;
        }

        public bool GetSfxDefinition(AudioSfxType type, out AudioSfxDefinition definition)
        {
            if (sfxLookup.TryGetValue(type, out definition))
                return true;
    
            DebugLogger.Log($"Sfx type {type} not found!", LogCategory.Framework, LogLevel.Error);
            return false;
        }
        
        private void CreateAudioLookup()
        {
            foreach (var data in audioDatabase.MusicDatabase)
            {
                musicLookup.Add(data.Type, data);
            }
            
            foreach (var data in audioDatabase.SfxDatabase)
            {
                sfxLookup.Add(data.Type, data);
            }
        }
        
        private void CreateDialogLookup()
        {
            foreach (var data in dialogCueDatabase.Database)
            {
                dialogCueLookup.Add(data.MissionIndex, data);
            }
        }
        
        private void CreateAvatarLookup()
        {
            foreach (var data in avatarDatabase.Database)
            {
                avatarLookup.Add(data.AvatarType, data);
            }
        }

        private void CreateRewardLookup()
        {
            foreach (var data in rewardConfig.RewardFactors)
            {
                rewardLookup.Add(data.RewardType, data);
            }
        }
        
        private void CreateUpgradeLookup()
        {
            foreach (var data in upgradeConfig.UpgradeDefinitions)
            {
                upgradeLookup.Add(data.UpgradeType, data);
            }
        }
        
        private void CreateLevelLookup()
        {
            for (var i= 0; i < missionRequirementsDatabase.Database.Count; i++)
            {
                levelLookup.Add(i, missionRequirementsDatabase.Database[i]);
            }
        }
        
        private void CreatePickupLookup()
        {
            foreach (var data in pickupDatabase.PickupDatas)
            {
                pickupLookup.Add(data.Type, data);
            }
        }
        
        private void CreateWeaponLookup()
        {
            foreach (var data in weaponDatabase.Database)
            {
                weaponLookup.Add(data.Type, data);
            }
        }
 
        private void CreateVehicleLookup()
        {
            foreach (var vehicleData in vehicleDatabase.Database)
            {
                vehicleLookup.Add(vehicleData.Guid, vehicleData);
            }
        }
        
        public LevelDefinition[] GetAllLevelDefinitions()
        {
            return levelLookup.Values.ToArray();
        }
        
        public bool GetLevelDefinition(int levelIndex, out LevelDefinition definition)
        {
            if (levelLookup.TryGetValue(levelIndex, out definition))
                return true;
    
            DebugLogger.Log($"Level index {levelIndex} not found!", LogCategory.Framework, LogLevel.Warning);
            return false;
        }

        public bool GetUpgradeDefinition(UpgradeType type, out UpgradeDefinition definition)
        {
            if (upgradeLookup.TryGetValue(type, out definition))
                return true;
    
            DebugLogger.Log($"Upgrade type {type} not found!", LogCategory.Framework, LogLevel.Error);
            return false;
        }

        //TODO: DO WE NEED TO LOOKUP EVERY TIME? SHOULD WE STORE IN A DICT?
        public DialogTrackDefinition? GetDialogSequenceDefinition(int missionIndex, WorldStateType type)
        {
            if (!dialogCueLookup.TryGetValue(missionIndex, out var config))
                return null;

            foreach (var c in config.DialogCues)
            {
                if (c.worldStateType == type)
                    return c.dialogTrackData.DialogTrackDefinition;
            }
            
            return null;
        }

        public WeaponDefinition[] GetAllWeaponData()
        {
            return weaponLookup.Values.ToArray();
        }

        public VehicleData[] GetAllVehicleDefinition()
        {
            return vehicleLookup.Values.ToArray();
        }

        public bool GetVehicleData(string id, out VehicleData data)
        {
            if (vehicleLookup.TryGetValue(id, out data))
                return true;
    
            DebugLogger.Log($"Vehicle ID {id} not found!", LogCategory.Framework, LogLevel.Error);
            return false;
        }

        public Sprite GetAvatarSprite(AvatarType type)
        {
            if (avatarLookup.TryGetValue(type, out var definition))
                return definition.Sprite;
    
            DebugLogger.Log($"Avatar type {type} not found!", LogCategory.Framework, LogLevel.Error);
            return null;
        }
        
        public MissionDefinition GetMissionDefinition(int index)
        {
            return missionDatabase.Database[index].definition;
        }

        public int GetRewardAmount(int missionIndex, RewardType rewardType)
        {
            if (missionIndex >= rewardConfig.MissionRewardBaseValues.Count)
            {
                DebugLogger.Log($"Mission Index is out of range. Value: {missionIndex}, " +
                                $"Max: {rewardConfig.MissionRewardBaseValues.Count}", 
                    LogCategory.Framework, LogLevel.Warning);
                return 0;
            }
            
            if (!rewardLookup.TryGetValue(rewardType, out var value))
            {
                DebugLogger.Log($"Reward category {rewardType} not found.", LogCategory.Framework, LogLevel.Warning);
                return 0;
            }
            
            return Mathf.FloorToInt(rewardConfig.MissionRewardBaseValues[missionIndex] * value.Factor);
        }
        
        public bool GetWeaponDefinition(WeaponType type, out WeaponDefinition definition)
        {
            if (weaponLookup.TryGetValue(type, out definition))
                return true;
            
            DebugLogger.Log($"Weapon type {type} not found!", LogCategory.Framework, LogLevel.Warning);
            return false;
        }
 
        public bool GetPickupDefinition(PickupType type, out PickupDefinition definition)
        {
            if (pickupLookup.TryGetValue(type, out definition))
                return true;
            
            DebugLogger.Log($"Pickup type {type} not found!", LogCategory.Framework, LogLevel.Warning);
            return false;
        }
        
        public override void Shutdown()
        {
            weaponLookup.Clear();
            vehicleLookup.Clear();
            rewardLookup.Clear();
            pickupLookup.Clear();
            levelLookup.Clear();
            musicLookup.Clear();
            sfxLookup.Clear();
            upgradeLookup.Clear();
            dialogCueLookup.Clear();
            avatarLookup.Clear();
        }

        public override void Initialize()
        {
            CreatePickupLookup();
            CreateVehicleLookup();
            CreateWeaponLookup();
            CreateRewardLookup();
            CreateAudioLookup();
            CreateLevelLookup();
            CreateUpgradeLookup();
            CreateDialogLookup();
            CreateAvatarLookup();
        }
    }
}
