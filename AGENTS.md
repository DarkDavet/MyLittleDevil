# AGENTS.md

This file provides guidance to agents when working with code in this repository.

## Project Overview
Unity 2D game (2021.3.16f1, Built-In Render Pipeline) — Flappy Bird-inspired adventure game. Play as a devil collecting items, casting spells, summoning minions, battling angels.

## Key Architecture Patterns
- GameStateMachine uses `RequestState<T>()` extension — states registered via `AddState()` in GameStateController
- State flow: TutorialGameState → RunGameState ↔ FightGameState → (LoseGameState | WinGameState)
- Abstract Factory: MinionFactory → RedMinionFactory/BlueMinionFactory (creates based on player aspect)
- Command Pattern: ICommand → MoveCommand for undoable actions via CommandManager
- Object Pooling: PoolManager with string tags — spawned objects implement IPooledObject
- SceneData assets (DLG_1-7) define narrative — DLG_1=prologue, DLG_2-6=intermissions, DLG_7=outro

## File Structure
- `Assets/Scripts/` — Core game logic (combat, AI, factories, state machine)
- `Assets/Prefabs/` — Game entities (enemies, items, minions, UI modules)
- `Assets/Scenes/` — MainMenu.unity, Level1-7.unity, DLG_Scene_1.unity

## Unity-Specific Notes
- Game events via static `GameEvents` singleton — subscribe in Awake(), unsubscribe in OnDestroy()
- TimeManager affects global `Time.timeScale` — all time-dependent code must account for slowdown
- Scene management uses SceneData ScriptableObject assets (guid: c7b5c6847b5ac6e4c84e83784a35d5d7)
- UI organized into separate Canvas groups for performance
- All 2D assets organized into sprite atlases

## How to Run
1. Clone the repository
2. Open in Unity 2021.3.16f1
3. Run scene `Assets/Scenes/MainMenu.unity`

## Important Components
- `Player.cs` — Player controller with jump mechanics, implements ICollectibleCollector
- `PlayerHealthSystem.cs` — Health management (maxHealth=3 hardcoded in Start())
- `GameStateController.cs` — Central hub holding ALL singleton references (Player, Camera, UI, etc.)
- `PoolManager.cs` — Object pooling via string-tagged dictionaries
- `EntryPoint.cs` — Scene entry point, initializes SceneData, requests TutorialGameState

## Game Flow
MainMenu → TutorialGameState → RunGameState → FightGameState → (Lose/Win) → MainMenu

## Mode-Specific Rules
See `.roo/rules-*/AGENTS.md` for detailed coding, debugging, documentation, and architecture rules.