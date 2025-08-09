using System.Linq;
using Debug_Module;
using Framework_Module.Enums;
using Framework_Module.Core;
using Framework_Module.Event;
using Framework_Module.Event.System;
using Framework_Module.Extensions;
using Framework_Module.Interfaces;
using Framework_Module.Scenes;
using Framework_Module.Service;
using UnityEngine;

namespace Framework_Module.Game_State
{
    public class BootGameState : GameState
    {
        private readonly EventBus eventBus;
        
        public BootGameState(SceneDirector sceneDirector, GameStateManager gameStateManager, IInputController inputController, EventBus eventBus) :
            base(sceneDirector, gameStateManager, inputController)
        {
            this.eventBus = eventBus;
        }

        public override GameStateType GameStateType => GameStateType.Boot;

        public override void Enter()
        {
            Time.timeScale = 1;
            eventBus.Subscribe<ServiceInitCompleteEvent>(OnServiceInitComplete);
            DebugLogger.Log("Boot Started", LogCategory.Framework, LogLevel.Log);
        }

        private async void OnServiceInitComplete(ServiceInitCompleteEvent e)
        {
            DebugLogger.Log("Boot Complete", LogCategory.Framework, LogLevel.Log);
#if UNITY_EDITOR
            const string key = "lastPlayScene";
            if (UnityEditor.EditorPrefs.HasKey(key))
            {
                var sceneName = UnityEditor.EditorPrefs.GetString(key);
                var sceneType = sceneName.SceneNameToType();
                if (sceneType == SceneType.Gameplay)
                {
                    var gameData = Services.Instance.Get<IGameData>();
                    var configDatabase = Services.Instance.Get<IConfigDatabase>();
                    gameData.TransientPlayerData.SetWeaponSelections(configDatabase.GetAllWeaponData().Where(data => data.CanBePurchased).ToArray());
                    gameData.TransientPlayerData.SetMissionSelection(0);
                    gameData.TransientPlayerData.SetVehicleSelection(VehicleArchetype.F14);
                }
                await SceneDirector.Transition(sceneType);
                GameStateManager.ChangeState(sceneName.SceneNameToStartingGameState());
                return;
            }
#endif
            await SceneDirector.Transition(SceneType.Splash);
            GameStateManager.ChangeState(GameStateType.Splash);
        }

        public override void Exit()
        {
            eventBus.Unsubscribe<ServiceInitCompleteEvent>(OnServiceInitComplete);
        }
    }
}