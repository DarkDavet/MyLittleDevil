# Project Debug Rules (Non-Obvious Only)

- GameState transitions are silent — add `Debug.Log($"[State] {typeof(T).Name}")` in StateExtensions.cs to trace state changes
- PoolManager spawns fail silently with warning — check Console for "Pool with tag X doesn't exist!" warnings
- TimeManager's `TakeItSlow()` affects global Time.timeScale — if physics behaves unexpectedly, check if slow-motion was triggered
- AudioManager.Play() silently fails if sound name not found — verify sound names match exactly (case-sensitive)
- SceneData assets (DLG_1_Scene through DLG_7_Scene) are ScriptableObject references — broken references show as `{fileID: 0}` in EntryPoint.cs
- `FindObjectOfType<AudioManager>()` returns null if AudioManager prefab not in scene — always check for null before use
- GameState IsFinalState property prevents state transitions — check which state sets this (LoseGameState, WinGameState)
- Object pooling uses Queue — objects return in FIFO order, which affects which pooled instance gets reused
- PlasticSCM is configured as version control (ProjectSettings) — do not use git for Unity asset changes
- VisualScripting package is included but unused — ignore .cs files in VisualScripting directories