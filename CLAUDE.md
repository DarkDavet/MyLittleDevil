# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**FlappyLittleDevil** — a Unity 2021.3 (URP 12.1.8) mobile game. The player flies through procedurally scrolling levels, dodging obstacles and collecting items. The game features a time-rewind mechanic, a customization shop, collectibles, achievements, and a dialogue/tutorial system. Built in C# with no separate build pipeline — standard Unity Editor workflow.

## Key Directories

| Directory | Purpose |
|---|---|
| `Assets/Scripts/` | Core game logic |
| `Assets/Scripts/GameStateMachine/` | State machine architecture (central to game flow) |
| `Assets/Scripts/Player/` | Player movement, health, shooting |
| `Assets/Scripts/Enemies/` | AI enemies, bosses, minions, projectiles |
| `Assets/Scripts/UI/` | UI managers, health bars, input controllers |
| `Assets/Scripts/UI/WindowManagement/` | Generic window manager with DOTween animations |
| `Assets/Scripts/Managers/` | Global managers (Audio, Scene, Coroutine, Pool) |
| `Assets/Scripts/CommandPattern/` | Undo/redo system for time-reverse feature |
| `Assets/Scripts/CustomizationSystem/` | Item shop, equipping, visual preview |
| `Assets/ScriptableObjects/` | Item data, inventory data |
| `Assets/Prefabs/` | Scene prefabs (organized by category) |
| `Assets/Scenes/` | All scenes (MainMenu, Levels 0–12, DLG 0–9, Credits, LevelTest) |
| `Assets/TestFeatures/` | AchievementSystem, CollectibleSystem, TutorialSystem, DialogueSystem |
| `Assets/LoadedAssets/` | Third-party assets ( Behaviour Tree, impact FX, fire sprites, Low Poly Fire) |
| `Assets/InputSystem/` | Unity Input System bindings and handler |
| `Assets/Plugins/Demigiant/DOTween/` | DOTween for animations |

## Architecture

### Game State Machine (central orchestrator)

The game flow is controlled by a **state machine** pattern. Key classes:

- `GameStateController` — singleton-like controller that holds references to Player, Inventory, Camera, Input, UI, SceneLoader, TutorialSystem, TimeReverseController. Manages state transitions via `OnStateChangeRequest` static event.
- `GameState` (abstract) — base class with `Enter()`, `Update()`, `Exit()` lifecycle. States:
  - `RunGameState` — normal scrolling gameplay
  - `FightGameState` — boss encounter
  - `PauseGameState` — time is paused
  - `WinGameState` — level complete (final state, blocks further transitions)
  - `LoseGameState` — game over (final state)
  - `TutorialGameState` — introductory tutorial
- `GameStateContext` — MonoBehaviour that wires up `GameStateController` with injected references and registers all states. Calls `_stateController.Update()` every frame.
- `StateExtensions` — extension methods (`this.RequestState<T>()`, `this.RequestPreviousState()`) for clean state transitions from any MonoBehaviour.

### Time Reverse System

A core gameplay mechanic. `TimeReverseController` records player/camera transform and health state every `FixedUpdate` using the Command Pattern (`MoveCommand`, `HealthCommand`). When the player activates time reverse (bound to an input action), it replays commands in reverse. `TimeManager` handles time scaling (full slowdown vs. player-excluded slowdown) and restore.

### Command Pattern

- `ICommand` interface: `Execute()`, `Undo()`
- `MoveCommand` — stores and restores Transform position/rotation
- `HealthCommand` — stores and restores PlayerHealthSystem health value
- `CommandManager` — `LimitedStack<ICommand>` with `ExecuteCommand()`, `UndoLastCommand()`, `ClearHistory()`

### Entity Systems

**Player** (`Player`): Rigidbody2D physics-based flight, rotation tilt based on velocity, collision-based damage. Implements `ICollectibleCollector`.

**Enemies**:
- `BaseAIBehaviour` — shared movement (bouncing Y axis) and fire rate fields
- `Minion` (abstract) — extends BaseAIBehaviour, adds X movement, attack detection, lifetime
- `RedMinion` / `BlueMinion` — colored variants with different `Attack()` implementations
- `BossBehaviour` / `BossShooting` — boss AI
- `EnemyArrow` / `BossProjectile` / `PlayerFireProjectile` / `PlayerIceProjectile` — projectiles

**Object Pooling**: `PoolManager` singleton pre-instantiates pools per tag. Spawned objects can implement `IPooledObject` for spawn-time setup.

### Collectibles & Achievements

- `CollectibleManager` (singleton, DontDestroyOnLoad) — tracks collected items and unique IDs via `PlayerPrefs`. Supports per-type save behavior (OnCollection, OnLevelComplete, OnGameEnd). Merges temporary items into persistent on level complete.
- `AchievementManager` — not a singleton (access via `AchievementSystemCore.Instance.AchievementManager`). Registers `AchievementType` ScriptableObject assets, tracks progress, unlocks. Saves to `PlayerPrefs`. Uses listener pattern (`IAchievementListener`) for UI decoupling.
- `Collectible` / `CollectibleType` — ScriptableObjects defining collectible data and save behavior.

### Customization System

Player can buy and equip cosmetic items (Hats, Glasses, Jewelry) using in-game currency (collected items like coins). `CustomizationManager` handles purchase flow, equip/unequip, and UI coordination. `CustomizationItem` (ScriptableObject) defines each item's properties, price, and currency type. `CharacterPreview` applies equipped items visually to the player. `PlayerVisuals` handles the visual components.

### Input System

Unity Input System (`PlayerControls` generated class) mapped in `PlayerInputHandler`. Supports custom key rebinding persisted via `PlayerPrefs` (JSON overrides). Jump, fire, ice-shoot, and 3 item slots.

### Dialogue System

Located in `Assets/TestFeatures/DialogueSystem/`. `DialogueSetup` (ScriptableObject) holds dialogue entries. `DialogueSystem` manages flow. `DLG_EntryPoint` is the scene-level trigger. Dialogue scenes (DLG_0 through DLG_9) are separate scenes loaded between levels.

### Settings System

Located in `Assets/Scripts/Settings/`. A pluggable subsystem architecture for game settings. `SettingsManager` is the only singleton and singleton orchestrator; subsystems are child components that implement `ISettingsSubsystem`. Logic layer is fully decoupled from UI via events.

**Core files:**

| File | Purpose |
|---|---|
| `ISettingsSubsystem.cs` | Interface — `Initialize()`, `CacheCurrentState()`, `ApplyAndSave()`, `DiscardChanges()`, `ResetToDefault()`, `HasUnsavedChanges()`, `RefreshUIRequested` event |
| `SettingsManager.cs` | Singleton — registers subsystems, iterates over them for all operations |
| `InputSettingsManager.cs` | Input binding subsystem — loads/saves/rejects JSON overrides via Unity Input System, fires `RefreshUIRequested` |
| `RebindButton.cs` | UI component — interactive key rebinding via `PerformInteractiveRebinding()`, subscribes to `RefreshUIRequested` via `SettingsManager.Instance.inputSettings` |
| `GraphicsSettingsManager.cs` | Graphics settings subsystem — manages quality level, VSync, frame rate, fullscreen, resolution. Resolution stored as JSON (width/height/fullscreen) for cross-device portability. Fires `RefreshUIRequested` |

**Architecture:**

```
SettingsManager (singleton, DontDestroyOnLoad)
  ├── InputSettingsManager ──┐
  └── GraphicsSettingsManager┼── ISettingsSubsystem
                             └── RefreshUIRequested event ──→ UI listeners
```

- Only `SettingsManager` is a singleton. Subsystems are registered via the Unity inspector and added to `_subsystems` list.
- Each subsystem fires `RefreshUIRequested` when its state changes. UI components subscribe/unsubscribe in `OnEnable`/`OnDisable`.
- Changes are batched: `CacheCurrentState()` is called on window open, `ApplyAndSave()` persists on "Apply" click, `DiscardChanges()` rolls back to cached snapshot.
- `RebindButton` is self-contained (uses `GetComponent<Button>()` in `Awake()`), subscribes via `SettingsManager.Instance.inputSettings.RefreshUIRequested`.
- `GraphicsSettingsManager` uses `GetAvailableResolutions()` (platform-aware: mobile returns single resolution, PC returns `Screen.resolutions`), `FormatResolutionWithFullscreen()` for display text, `SetResolution(int)` saves width/height/fullscreen as JSON. Resolution lookup on load searches `Screen.resolutions` and falls back to native if not found.

**UI layer:** `SettingsWindow` extends `UIWindow`, owns all UI refs, delegates logic to `SettingsManager`. Subscribes to both subsystems' events.

### Window Management

Located in `Assets/Scripts/UI/WindowManagement/`. A singleton-based window system for screen-like UIs (main menu, settings, shop, achievements, etc.). Windows are registered in the Unity inspector and controlled by `WindowID`. Each window animates with a fade + scale effect via DOTween. `SetUpdate(true)` on all DOTween calls ensures animations run even when `Time.timeScale == 0`.

**Core files:**

| File | Purpose |
|---|---|
| `WindowID.cs` | Enum — unique window identifiers (`None`, `Main`, `Levels`, `Shop`, `Achievements`, `Settings`) |
| `UIWindow.cs` | Abstract base — `Open()` (fade-in + scale-up), `Close()` (scale-down + fade-out → deactivate), `OnOpen()`/`OnClose()` hooks |
| `UIWindowsManager.cs` | Singleton — registers windows by `WindowID`, open/close routing, auto-fallback to Main on close |
| `WindowButton.cs` | Attach to Button GameObject — opens a selected `WindowID` on click |

**`UIWindowsManager` public methods:**
- `OpenWindow(id)` — opens a window by ID (closes current first)
- `OpenWindowFromButton(id)` — inspector-friendly overload for Button.OnClick (enum dropdown)
- `CloseCurrentWindow()` — closes current window, then opens Main as fallback
- `IsWindowOpen(id)` — returns true if a window with the given ID is active

**Window implementations:**
- `AchievementsWindow` — **fully implemented**. Slot management, count display, `IAchievementListener` for live updates. Inspector fields: `slotPrefab`, `slotsContainer`, `totalCountText`, `unlockedCountText`.
- `SettingsWindow` — **fully implemented**. Extends `UIWindow`, owns all UI refs. Delegates logic to `SettingsManager`. Subscribes to `InputSettingsManager.RefreshUIRequested` and `GraphicsSettingsManager.RefreshUIRequested` via `OnEnable`/`OnDisable`. Initializes dropdowns in `OnOpen()` (not `OnEnable` to avoid double-build). Methods: `SetQualityLevel(int)`, `SetVSync(bool)`, `SetFrameRate30/60/Unlocked()`, `SetFullscreen(bool)`, `SetResolution(int)`, `ApplySettings()`, `ResetSettings()`, `TryClose()`, `ConfirmDiscard()`. Inspector fields: `settingsManager`, `graphicsSettings`, `qualityDropdown` (TMP_Dropdown), `vSyncToggle` (Toggle), `frameRate30/60/UnlockedButton` (Button), `fullscreenToggle` (Toggle), `resolutionDropdown` (TMP_Dropdown), `confirmationPopup` (GameObject).
- `MainMenuWindow`, `LevelsWindow`, `ShopWindow` — **placeholders** with `OnOpen()` TODO comments.

### Scenes

Build order in `EditorBuildSettings`: MainMenu → DLG_0 → Level_0 → DLG_1 → Level_1 → ... → DLG_9 → Level_12 → Credits. Level tests via `LevelTest.unity`. Scene transitions use `SceneLoader` with `SceneData` (ScriptableObject) for level metadata and collectible stats.

## Development Workflow

- **Open in Unity Editor** — this is a standard Unity project. No external build scripts.
- **Run** — press Play in the Unity Editor. MainMenu.unity is the entry scene.
- **Add a new level** — create a new scene, add an `EntryPoint` prefab, create a `SceneData` ScriptableObject, add to `EditorBuildSettings`.
- **Add a new achievement** — create an `AchievementType` asset, register it via `AchievementSystemCore`.
- **Add a new collectible** — create a `CollectibleType` asset with desired `SaveBehavior`.
- **Add a new enemy** — extend `BaseAIBehaviour` or `Minion`, create a prefab, add to the scene.
- **Add a new game state** — extend `GameState`, register in `GameStateContext.Init()`, handle in `GameStateContext.SetInitState()`.

## Coding Conventions

- No namespace usage in `Assets/Scripts/` (flat namespace). `AchievementSystem` and `CollectibleSystem` namespaces used only in `TestFeatures/`.
- Hungarian notation for private fields (`_fieldName`), standard C# for public/protected (`FieldName`).
- `SerializeField` for inspector-exposed private fields.
- Static events for cross-object communication (`GameEvents`, `GameStateController.OnStateChangeRequest`).
- `DontDestroyOnLoad` used for singleton managers (AudioManager, PoolManager, TimeManager, CollectibleManager, AchievementSystemCore).
- `PlayerPrefs` used for all persistence (achievements, collectibles, customization, custom bindings, level unlocks).
- Russian comments appear in some files (TimeManager, PlayerInputHandler, AchievementManager) — these are developer notes, not user-facing text.

## Dependencies (key packages)

- Unity URP 12.1.8
- Unity Input System 1.4.4
- DOTween (Demigiant)
- Unity Addressables 1.19.19
- Behaviour Tree (TheKiwiCoder — loaded asset, not used in production code)
