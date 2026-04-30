# Project Coding Rules (Non-Obvious Only)

- GameState transitions via `this.RequestState<T>()` extension on any object (StateExtensions.cs) — never instantiate GameState directly
- `GameStateController` holds ALL singleton references (Player, CameraMoving, UIManager, etc.) — access game objects through `GameStateController` instance, not `FindObjectOfType`
- `GameEvents` uses static events with Trigger methods — subscribe in Awake(), unsubscribe in OnDestroy() to prevent memory leaks (see PlayerHealthSystem.cs)
- Object pooling via `PoolManager` uses string tags — prefabs must have correct Tag set matching pool entry (PoolManager.cs line 43-64)
- MinionFactory is abstract base — concrete implementations (RedMinionFactory, BlueMinionFactory) follow same CreateMinion pattern
- CommandManager uses LimitedStack<ICommand> for undo — commands must implement both Execute() and Undo() (CommandPattern/)
- TimeManager uses DontDestroyOnLoad — only one TimeManager instance allowed across scenes
- AudioManager uses string-based sound lookup via Array.Find — sound names must exactly match clip names in the sounds array
- SceneData assets (DLG_1_Scene through DLG_7_Scene) use script guid c7b5c6847b5ac6e4c84e83784a35d5d7 — do not modify the guid
- Player implements ICollectibleCollector interface — new collectible types must implement this interface
- `Bar[]` in AspectManager is serialized field — order determines UI toggle sequence (V key toggles fire/ice aspect)
- All MonoBehaviour singletons use `Instance` property pattern with Awake() initialization — never use static constructors