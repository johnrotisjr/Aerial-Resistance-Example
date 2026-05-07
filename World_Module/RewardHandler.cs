using System;
using Framework_Module.Core;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.Combat;
using Framework_Module.Event.Objective;
using Framework_Module.Interfaces;
using World_Module.WorldObjects;

namespace World_Module
{
    public class RewardHandler : IDisposable
    {
        private readonly IConfigDatabase configDatabase;
        private readonly EventBus eventBus;
        private readonly IPlayerController playerController;
        private readonly IGameData gameData;
        
        public RewardHandler(IPlayerController playerControllerService, IConfigDatabase configDatabaseService,
            EventBus eventBusService, IGameData gameData)
        {
            configDatabase = configDatabaseService;
            eventBus = eventBusService;
            playerController = playerControllerService;
            this.gameData = gameData;
            eventBus.Subscribe<WorldObjectKilledEvent>(OnWorldObjectDestroyed);
            eventBus.Subscribe<ObjectiveCompleteEvent>(OnObjectiveComplete);
            eventBus.Subscribe<BossDestroyedEvent>(OnBossDestroyed);
        }
        
        private void AddCash(RewardType type)
        {
            var rewardAmount = configDatabase.GetRewardAmount(gameData.TransientPlayerData.SelectedMissionIndex, type);
            gameData.PersistentPlayerData.AddCash(rewardAmount);
        }

        private void OnBossDestroyed(BossDestroyedEvent e)
        {
            AddCash(RewardType.BossKill);
        }

        private void OnWorldObjectDestroyed(WorldObjectKilledEvent e)
        {
            if (e.WorldObject is not Vehicle vehicle || vehicle.AlignmentType != AlignmentType.Foe)
            {
                return;
            }
            
            AddCash(RewardType.EnemyKill);
        }
        
        private void OnObjectiveComplete(ObjectiveCompleteEvent e)
        {
            switch (e.State.ObjectiveType)
            {
                case ObjectiveType.Primary:
                    AddCash(RewardType.PrimaryObjective);
                    break;
                case ObjectiveType.Secondary:
                    AddCash(RewardType.SecondaryObjective);
                    break;
                case ObjectiveType.Hidden:
                    AddCash(RewardType.HiddenObjective);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
 
        public void Dispose()
        {
            eventBus.Unsubscribe<WorldObjectKilledEvent>(OnWorldObjectDestroyed);
            eventBus.Unsubscribe<ObjectiveCompleteEvent>(OnObjectiveComplete);
            eventBus.Unsubscribe<BossDestroyedEvent>(OnBossDestroyed);
        }
    }
}