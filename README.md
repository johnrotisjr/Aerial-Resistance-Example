# Aerial Resistance — Example Modules

This repository contains two code modules from **Aerial Resistance**, a side-scrolling shooter developed with Unity. These modules are provided to demonstrate the game's **AI patterns** and **core framework architecture**. This is not the complete game.

-----


### Scope

#### Included

  * **`Ai_Module/`**: AI behaviors and spawning utilities.
  * **`Framework_Module/`**: Core services, event system, state management, scene utilities, and configuration types.

#### Not Included

  * Other game modules (audio, input, UI, world, etc.), Unity scene assets (`.unity`), and art/audio content.

#### Cross-Module Reference

Some code references the debug/logging module, which is not included here.

-----

### Directory Layout

```
Ai_Module/
Framework_Module/
```

### What’s Included

#### AI Module (`Ai_Module/`)

This module contains AI behaviors and the data/config types that drive enemy movement and attacks.

  * **Top-level**: `AiBehaviorExecutor.cs`, `AiBehaviorFlowController.cs`, `AiSpawnPositioner.cs`
  * **Behaviors**:
      * **Attack** (`Behavior` & `Config`): `NoAttackBehavior`, `RadialAttackBehavior`, `SniperAttackBehavior`, `SprayAttackBehavior`, `StraightAttackBehavior`
      * **Movement** (`Behavior` & `Config`): `Arching`, `Hover`, `Kamikaze`, `MoveToPosition`, `NoMovement`, `Orbit`, `Retreat`, `Stalker`, `Straight`, `Wobble`

#### Framework Module (`Framework_Module/`)

This is the core framework used across the game.

  * **Core**: `GameCoordinator`, `SceneDirector`, `CoroutineRunner`, `GameObjectPooler`, `Pool`, `Cache`, `SingletonMonoBehavior`
  * **Services**: Services facade, `ServiceLocator`, `ServiceLifecycleManager`, `GameServiceBase`
  * **Event System**: `EventBus` and events for:
      * **Combat**: `WorldObjectDestroyedEvent`, `DamageTakenEvent`, `BossDestroyedEvent`
      * **Objective**: `ObjectiveCompleteEvent`
      * **Gameplay**: `RestartMissionEvent`, `ExitMissionEvent`, `SpawnWeaponEvent`
      * **State**: `GameStateChangeEvent`, `WorldStateChangeEvent`, `PauseEvent`, `GameOverEvent`, `GoBackEvent`
      * **System**: `ServiceInitCompleteEvent`, `SceneLoadCompleteEvent`, `LoadingProgressEvent`, `ResolutionChangedEvent`
      * **UI**: `DisplayScreenEvent`, `HideScreenEvent`, `NewGameSelectedEvent`, `UpdateHudEvent`
      * **Root Events**: `MissionSelectedEvent`, `PlaneSelectedEvent`, `PlaySfxEvent`, `PickupCollectedEvent`, `WeaponsSelectedEvent`
  * **Game State**: `GameStateManager`, base `GameState`, and concrete states: `BootGameState`, `SplashGameState`, `TitleGameState`, `MenuGameState`, `GameplayGameState`, `MissionHubGameState`, `LoadingGameState`
  * **Scenes (code utilities)**: `SceneLoader`, `SceneTransitionController` (no `.unity` scene files in this repo)
  * **Configs**: `ConfigDatabase` and various configuration files for `Audio`, `Avatar`, `Dialog`, `Level`, `Mission`, `Reward`, `Upgrade`, and `Ai`/`Entities`.
  * **Definitions**: `AiRoutineDefinition`, `AiPhaseDefinition`, `DialogEntryDefinition`, `LevelDefinition`, `MissionDefinition`, `PickupDefinition`, etc.
  * **Enums & Interfaces**: Numerous enums (e.g., `GameStateType`, `SceneType`) and interfaces (e.g., `IGameService`, `IBehavior`).

### License

**Copyright © 2025**

John R. Otis Jr.
No Way Out Games LLC
All rights reserved.

This codebase and all associated assets, scripts, and intellectual property are the exclusive property of John R. Otis Jr. and No Way Out Games LLC.
