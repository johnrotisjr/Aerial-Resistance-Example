using System;
using Framework_Module.Enums;

namespace Framework_Module.Extensions
{
    public static class SceneNameExtensions
    {
        public static SceneType SceneNameToType(this string sceneName)
        {
            return sceneName switch
            {
                "GameplayScene" => SceneType.Gameplay,
                "BootScene" => SceneType.Boot,
                "TitleScene" => SceneType.Title,
                "MenuScene" => SceneType.Menu,
                "LoadingScene" => SceneType.Loading,
                "DebugScene" => SceneType.Debug,
                "MissionHubScene" => SceneType.MissionHub,
                "SplashScene" => SceneType.Splash,
                _ => throw new ArgumentOutOfRangeException(nameof(sceneName), sceneName, null)
            };
        }
        
        public static GameStateType SceneNameToStartingGameState(this string sceneName)
        {
            return sceneName switch
            {
                "GameplayScene" => GameStateType.Gameplay,
                "BootScene" => GameStateType.Boot,
                "TitleScene" => GameStateType.Title,
                "MenuScene" => GameStateType.Menu,
                "LoadingScene" => GameStateType.Loading,
                "DebugScene" => GameStateType.Boot,
                "MissionHubScene" => GameStateType.MissionHub,
                "SplashScene" => GameStateType.Splash,
                _ => throw new ArgumentOutOfRangeException(nameof(sceneName), sceneName, null)
            };
        }
        
        public static string SceneTypeToNamed(this SceneType sceneType)
        {
            return sceneType switch
            {
                SceneType.Gameplay => "GameplayScene",
                SceneType.Boot => "BootScene",
                SceneType.Title => "TitleScene",
                SceneType.Menu => "MenuScene",
                SceneType.Loading => "LoadingScene",
                SceneType.Debug => "DebugScene",
                SceneType.MissionHub => "MissionHubScene",
                SceneType.Splash => "SplashScene",
                _ => throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null)
            };
        }
    }
}