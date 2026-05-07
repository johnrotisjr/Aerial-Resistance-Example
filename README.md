# Aerial Resistance — Example Modules

This repository contains a code-only sample from **Aerial Resistance**, a side-scrolling shooter developed with Unity. It is intended to demonstrate the game's modular runtime architecture, including AI, framework services, world simulation, UI, input, audio, data, debug, and scene bootstrap code.

This is not the complete game.

-----

## Scope

### Included

* **`Ai_Module/`**: Enemy behavior execution, behavior flow control, spawn positioning, and concrete attack/movement AI behaviors.
* **`Framework_Module/`**: Shared core systems, service infrastructure, events, game states, definitions, data objects, enums, helpers, interfaces, and scene utilities.
* **`World_Module/`**: Runtime world systems such as vehicles, weapons, pickups, missions, objectives, world states, collision handling, spawning, parallax helpers, and behavior factories.
* **`Ui_Module/`**: Screens, HUD elements, menu/loading/debug UI, selection widgets, button settings, and screen management.
* **`Input_Module/`**: Input binding, dispatch, controller, UI input handling, and input command implementations.
* **`Audio_Module/`**: Audio facade plus music and sound-effect players.
* **`Data_Module/`**: Runtime and persistent player/game data containers.
* **`Debug_Module/`**: Logging categories, levels, and logger implementation.
* **`Bootstrap_Module/`**: Scene bootstrap classes that wire services and runtime systems together.

### Not Included

* Unity scene files (`.unity`), prefabs, ScriptableObject assets, sprites, animations, shaders, audio clips, and other production content.
* Unity project settings, packages, generated library folders, and build artifacts.
* Unity `.meta` files and assembly definition files (`.asmdef`) are intentionally omitted from the current repository state.

Because this sample is source-only, it is meant for architecture/code review rather than as a drop-in runnable Unity project.

-----

## Directory Layout

```text
Ai_Module/
Audio_Module/
Bootstrap_Module/
Data_Module/
Debug_Module/
Framework_Module/
Input_Module/
Ui_Module/
World_Module/
```

-----

## What’s Included

### AI Module (`Ai_Module/`)

The AI module contains the runtime pieces that execute AI routines and individual behaviors.

* **Top-level controllers/utilities**: `AiBehaviorExecutor`, `AiBehaviorFlowController`, `AiSpawnPositioner`.
* **Attack behaviors**: `LaserSweepAttackAiBehavior`, `RadialAttackAIBehavior`, `SniperAttackAiBehavior`, `SprayAttackAiBehavior`, `StraightAttackAiBehavior`, `ThrustAttackAiBehavior`.
* **Movement behaviors**: `ArchingMovementAiBehavior`, `KamikazeMovementAiBehavior`, `LinearMovementAiBehavior`, `MaintainMovementAiBehavior`, `MoveToPositionMovementAiBehavior`, `OrbitMovementAiBehavior`, `PatrolMovementAiBehavior`, `StalkerMovementAiBehavior`, `TeleportMovementAiBehavior`, `WobbleMovementAiBehavior`.

Behavior configuration has moved out of the old per-behavior `Data` folders and into framework definitions/game-data types.

### Framework Module (`Framework_Module/`)

The framework module contains shared systems and abstractions used by the feature modules.

* **Core**: `GameCoordinator`, `SceneDirector`, `CoroutineRunner`, `GameObjectPooler`, `Pool`, `Cache`, `SingletonMonoBehavior`.
* **Services**: `Services`, `ServiceLocator`, `ServiceLifecycleManager`, `GameServiceBase`.
* **Event system**: `EventBus`, event subscriptions, and typed events for combat, objectives, gameplay flow, game/world state, services, scene loading, UI, root menu actions, audio, pickups, weapons, and resolution changes.
* **Game states**: Base state types and concrete states such as boot, splash, title, menu, gameplay, mission hub, and loading.
* **Definitions**: Data definitions for AI routines/phases, behavior groups, attack and movement behaviors, transition conditions, dialog, levels, missions, objectives, pickups, rewards, upgrades, vehicles, weapons, audio, and avatars.
* **Game data**: Runtime data classes for AI phases/routines, behavior groups, transition conditions, dialog, level flow, missions, objectives, rewards, upgrades, vehicles, weapons, and config/database access.
* **Enums and interfaces**: Shared contracts and identifiers for services, input, AI, world objects, vehicles, weapons, objectives, UI, game states, scene types, audio, pickups, and more.
* **Scenes**: Code utilities for scene loading and scene transitions. Unity scene assets are not included.
* **Debug**: Runtime debug overlay/support code.

### World Module (`World_Module/`)

The world module contains gameplay runtime systems for the mission space.

* **World objects**: Base world object support plus vehicles, weapons, weapon sockets, pickups, explosion views, tinting, and animation drivers.
* **Vehicle controllers**: Player, AI, and shared vehicle controller implementations.
* **Weapons and factories**: Attack, movement, and weapon behavior factories plus projectile/weapon movement behaviors for bullets, bombs, slugs, homing missiles, thrust weapons, and laser sweeps.
* **Mission/objective systems**: Mission manager, objective manager, objective state, rewards, and progress support.
* **World states**: Init, play, pause, game-over, base world state, and world state manager.
* **Environment/runtime helpers**: Battlefield manager, collision resolver, viewport bounds provider, world object spawner, parallax components, sprite bounds aggregation, and explosion event routing.

### UI Module (`Ui_Module/`)

The UI module contains reusable menu, screen, HUD, and control code.

* **Screens**: Dialog, map, mission complete, pause, plane selection, weapon selection, base game screen, and selection screen implementations.
* **HUD**: Health, weapon, currency, power level, mission progress, objective toast, base HUD element, and HUD orchestration.
* **Controls/utilities**: Game buttons and button settings, selection grids, vehicle information panels, UI extensions, debug UI, loading UI, menu UI, and screen manager.

### Input Module (`Input_Module/`)

The input module maps player/UI actions into commands.

* **Input systems**: `InputBindingManager`, `InputController`, `InputDispatcher`, `UiInputHandler`.
* **Commands**: Cancel, change weapon, debug, fire gun, fire weapon, move, navigate, pause, and submit commands.

### Audio Module (`Audio_Module/`)

The audio module provides audio service implementations.

* `Audio` acts as the audio facade/service.
* `MusicPlayer` handles music playback.
* `SfxPlayer` handles sound-effect playback.

### Data Module (`Data_Module/`)

The data module contains player and game data services.

* `GameData` provides game data access.
* `PersistentPlayerData` stores long-lived player progress.
* `TransientPlayerData` stores run/session state.
* `UpgradeData` tracks upgrade data.

### Debug Module (`Debug_Module/`)

The debug module contains logging support used across the sample.

* `DebugLogger` handles log output.
* `LogCategory` and `LogLevel` classify debug messages.

### Bootstrap Module (`Bootstrap_Module/`)

The bootstrap module contains composition roots for wiring scene-level dependencies.

* `GlobalBootstrapper` sets up global services.
* `GameSceneBootstrapper` wires gameplay scene systems.
* `MissionHubSceneBootstrapper` wires mission hub scene systems.

-----

## Notes for Reviewers

* The last module refresh renamed AI behaviors to the `*AiBehavior` convention and replaced older per-behavior config classes with framework definition/game-data objects.
* Additional feature modules are now included alongside the original AI/framework sample, so cross-module references to audio, input, UI, world, data, debug, and bootstrap code should resolve within the repository source.
* The repository intentionally remains code-focused and does not include the Unity assets needed to run the full game.

-----

## License

**Copyright © 2025**

John R. Otis Jr.<br>
No Way Out Games LLC<br>
All rights reserved.

This codebase and all associated assets, scripts, and intellectual property are the exclusive property of John R. Otis Jr. and No Way Out Games LLC.
