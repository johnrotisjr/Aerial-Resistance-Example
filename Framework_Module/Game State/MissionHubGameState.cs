using Framework_Module.Core;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.State;
using Framework_Module.Interfaces;
using UnityEngine.SceneManagement;

namespace Framework_Module.Game_State
{
    public class MissionHubGameState : GameState
    {
        public override GameStateType GameStateType => GameStateType.MissionHub;
        private readonly EventBus eventBus;
        private readonly IScreenManager manager;
        private readonly IGameData gameData;
        private readonly IAudio audio;
        
        public MissionHubGameState(IGameData gameData, SceneDirector sceneDirector, 
            GameStateManager gameStateManager, IInputController inputController, EventBus eventBus,
            IScreenManager manager, IAudio audio) : base(sceneDirector, gameStateManager, inputController)
        {
            this.eventBus = eventBus;
            this.manager = manager;
            this.gameData = gameData;
            this.audio = audio;
        }
        
        public override void Enter()
        {
            eventBus.Subscribe<MissionSelectedEvent>(OnMissionSelected);
            eventBus.Subscribe<GoBackEvent>(OnGoBack);
            eventBus.Subscribe<PlaneSelectedEvent>(OnPlaneSelected);
            eventBus.Subscribe<WeaponsSelectedEvent>(OnWeaponSelected);
            InputController.EnableUIControls();
            gameData.TransientPlayerData.Clear();
            manager.OpenScreen(ScreenType.Map);
            audio.MusicPlayer.Start(AudioMusicType.MenuMusic, true, 1);
        }
        
        private void OnMissionSelected(MissionSelectedEvent e)
        {
            gameData.TransientPlayerData.SetMissionSelection(e.MissionIndex);
            manager.OpenScreen(ScreenType.Plane);
        }
        
        private void OnPlaneSelected(PlaneSelectedEvent e)
        {
            if (e.Data.Length <= 0)
                return;
            
            gameData.TransientPlayerData.SetVehicleSelection(e.Data[0].VehicleArchetype);
            manager.OpenScreen(ScreenType.Weapon);
        }
        
        private async void OnWeaponSelected(WeaponsSelectedEvent e)
        {
            manager.CloseAllScreens();
            gameData.TransientPlayerData.SetWeaponSelections(e.Data);
            await SceneDirector.Transition(SceneType.Gameplay, LoadSceneMode.Single, true);
            GameStateManager.ChangeState(GameStateType.Gameplay);
        }

        private async void OnGoBack(GoBackEvent e)
        {
            await SceneDirector.Transition(SceneType.Menu);
            GameStateManager.ChangeState(GameStateType.Menu);
        }

        public override void Exit()
        {
            eventBus.Unsubscribe<MissionSelectedEvent>(OnMissionSelected);
            eventBus.Unsubscribe<GoBackEvent>(OnGoBack);
            eventBus.Unsubscribe<PlaneSelectedEvent>(OnPlaneSelected);
            eventBus.Unsubscribe<WeaponsSelectedEvent>(OnWeaponSelected);
            InputController.DisableUIControls();
        }
    }
}