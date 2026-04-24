# AGENTS.md

## Project Overview
This is a Unity 2D game project built with Unity 2021.3.16f1 using the Built-In Render Pipeline. The game is an adventure game inspired by Flappy Bird mechanics, featuring a player character that can collect items, cast spells, and battle angels.

## Key Architecture Patterns
- Abstract factory pattern for spawning projectiles and allied units based on player's chosen "Aspect"
- Base AI logic designed for easy scalability 
- Behavior tree system for complex AI decision-making
- Object pooling for frequently reused game objects
- State management system for game flow (Pause, Lose, Win)

## File Structure
- `Assets/Scripts/` - Core game logic including combat, AI and factories
- `Assets/Prefabs/` - Game entities including enemies, items, minions and UI modules
- `Assets/Scenes/` - Game scenes including MainMenu, Levels and Dialog scenes

## Unity-Specific Notes
- Game states are managed using a custom `RequestState<T>()` pattern
- Game events are handled via a `GameEvents` singleton
- Time management is controlled via `TimeManager` singleton
- Scene management uses a `SceneDatabase` and `SceneData` asset system
- UI is organized into separate Canvas groups for performance
- All 2D assets are organized into sprite atlases

## How to Run
1. Clone the repository
2. Open the project in Unity (2021.3.16f1)
3. Open and run the scene `Assets/Scenes/MainMenu.unity`

## Important Components
- `Player.cs` - Main player controller with jump mechanics and collision detection
- `PlayerHealthSystem.cs` - Health management with events
- `UIManager.cs` - UI management including pause functionality
- `TimeManager.cs` - Time manipulation effects for gameplay
- `SceneDatabase.cs` - Scene management system
- `GameEvents.cs` - Game event system

## Game Flow
- Game starts at MainMenu scene
- Level progression through SceneDatabase
- Player can collect items and use abilities
- Game state transitions handled through custom RequestState pattern