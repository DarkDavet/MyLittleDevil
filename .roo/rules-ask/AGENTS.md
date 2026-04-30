# Project Documentation Rules (Non-Obvious Only)

- SceneData assets (DLG_1_Scene through DLG_7_Scene) define the full game narrative — DLG_1 is prologue, DLG_2-6 are intermissions, DLG_7 is outro
- Level scenes (Level1-7) correspond to gameplay segments between dialogue scenes
- `Assets/Scenes/` contains: MainMenu.unity, Level1-7.unity, LevelTest.unity, DLG_Scene_1.unity (dialogue scenes)
- ScriptableObject SceneData uses guid c7b5c6847b5ac6e4c84e83784a35d5d7 — this is the SceneData class identifier
- Addressables system configured for asset management (com.unity.addressables 1.19.19) — items use ItemsAssets asset group
- `Assets/Scripts/Editor/CreateAssetBundle.cs` — custom editor script for building asset bundles (not used in runtime)
- UI prefabs organized by function: Canvas.prefab (main), Counters.prefab (HUD), Inventory/* (inventory system), Customization/* (customization)
- Manager prefabs in `Assets/Prefabs/Managers/` — these are the canonical instances placed in scenes, not the Scripts
- `Assets/Scripts/CollectibleSystem/` contains ICollectibleCollector interface — item collection system
- `Assets/Scripts/CustomizationSystem/` contains item customization logic — separate from gameplay items