# Project Architecture Rules (Non-Obvious Only)

- GameStateMachine uses RequestState<T>() extension pattern — states are registered in GameStateController via AddState(), not auto-discovered
- State flow: TutorialGameState → RunGameState ↔ FightGameState → (LoseGameState | WinGameState) — PauseGameState is interruptible
- `EntryPoint.cs` is the scene entry point — it initializes SceneData, calls gameStateManager.Init(), then requests TutorialGameState
- Abstract Factory pattern: MinionFactory (abstract) → RedMinionFactory, BlueMinionFactory (concrete) — creates minions based on player aspect
- Command Pattern: ICommand interface → MoveCommand implementation — used for undoable actions via CommandManager
- Object Pooling: PoolManager manages tagged pools — spawned objects must implement IPooledObject for spawn callbacks
- TimeManager affects global Time.timeScale — all time-dependent code must account for slowdown effects
- GameEvents is a static event bus — no dependency injection, all components subscribe directly to static events
- SceneDatabase pattern: SceneData assets define scene metadata (sceneName, isUnlockedByDefault, isDialogueScene)
- PlayerHealthSystem hardcodes maxHealth = 3 in Start() — health is not serialized, always resets to 3 on scene load
- AudioManager uses Array.Find for sound lookup — O(n) search, not Dictionary — adding 50+ sounds may impact performance
- PlasticSCM is the configured VCS (UnityProjectSettings) — do not assume git LFS for large assets