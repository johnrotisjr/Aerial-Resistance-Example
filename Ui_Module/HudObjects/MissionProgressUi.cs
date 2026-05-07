 
using System.Text;
 
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.State;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using TMPro;
using UnityEngine;

namespace Ui_Module.HudObjects
{
    /// <summary>
    /// Displays mission objectives in the UI and updates them based on objective completion events.
    /// Appears at the end of missions to show results.
    /// </summary>

    internal class MissionProgressUi : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        private EventBus eventBus;
        private Coroutine coroutine;
        private IGameData gameData;
        private IConfigDatabase configDatabase;
        private IObjectiveManager objectiveManager;

        public void Inject(EventBus eventBusService, IConfigDatabase configDatabaseService, 
            IGameData gameDataService, IObjectiveManager objectiveManagerService)
        {
            eventBus = eventBusService;
            objectiveManager = objectiveManagerService;
            gameData = gameDataService;
            configDatabase = configDatabaseService;
        }
        
        protected void Start()
        {
            eventBus?.Subscribe<GameOverEvent>(OnGameOver);
        }

        private void OnDestroy()
        {
            eventBus?.Unsubscribe<GameOverEvent>(OnGameOver);
            ResetObjectiveText();
        }

        public void UpdateUi()
        {
            ResetObjectiveText();
            CreateObjectiveText();
        }

        private void ResetObjectiveText()
        {
            text.text = string.Empty;
        }

        private void OnGameOver(GameOverEvent e)
        {
            UpdateUi();
        }
        
        private void CreateObjectiveText()
        {
            StringBuilder stringBuilder = new StringBuilder();
            var selectedMission = gameData.TransientPlayerData.SelectedMissionIndex;
            var enemyDestroyedCount = gameData.TransientPlayerData.EnemiesDestroyed;
            var killReward = configDatabase.GetRewardAmount(selectedMission, RewardType.EnemyKill);
            var totalEnemyDestroyedCash = killReward * enemyDestroyedCount;
            
            stringBuilder.Append($"Enemies destroyed {enemyDestroyedCount.ToString()}");
            if(enemyDestroyedCount > 0)
                stringBuilder.Append($" - ${totalEnemyDestroyedCash}");
            stringBuilder.AppendLine();
            
            var totalCashEarned = 0;
            foreach (var objectiveState in objectiveManager.AllObjectives)
            {
                var objectiveText = objectiveState.GetObjectiveText();
                var objectiveReward = configDatabase.GetRewardAmount(selectedMission, objectiveState.ObjectiveType.ObjectiveTypeToRewardType());
                stringBuilder.Append(objectiveText);
                if(objectiveState.IsComplete)
                {
                    totalCashEarned += objectiveReward;
                    stringBuilder.Append($"- ${objectiveReward.ToString()}");
                }
                stringBuilder.AppendLine();
            }
            
            var bossKillReward = configDatabase.GetRewardAmount(selectedMission, RewardType.BossKill) * gameData.TransientPlayerData.BossesDestroyed;
            if(bossKillReward > 0)
                stringBuilder.AppendLine($"Boss Destroyed cash Earned - ${bossKillReward}");
            
            totalCashEarned += totalEnemyDestroyedCash;
            totalCashEarned += bossKillReward;
            stringBuilder.AppendLine($"Total cash Earned - ${totalCashEarned}");

            text.text = stringBuilder.ToString();
        }
    }
}