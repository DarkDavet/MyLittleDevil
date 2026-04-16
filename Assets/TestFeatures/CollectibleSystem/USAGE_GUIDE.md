# Collectible System - Usage Guide

## Overview

The CollectibleSystem provides a complete solution for tracking collectible items in your game with proper persistence and different save behaviors.

## Key Features

1. **ScriptableObject-based Configuration**: Set total collectible counts in asset files
2. **Runtime Stats Tracking**: Collected/remaining counts are calculated at runtime and stored in PlayerPrefs
3. **Multiple Save Behaviors**: Items can be saved immediately, on level complete, on game end, or never
4. **Unique Collectible Support**: Prevent duplicate collection of special items using unique IDs
5. **Level-specific Tracking**: Track collectibles per level independently

## Setup

### 1. Create Collectible Types

1. Right-click in Project window → Create → Collectible System → Collectible Type
2. Configure each type:
   - **ID**: Unique identifier (e.g., "coin", "gem", "heart")
   - **Display Name**: User-friendly name for UI
   - **Icon**: Sprite to display in UI
   - **Save Behavior**: When to save progress
     - **OnCollection**: Save immediately (permanent items)
     - **OnLevelComplete**: Save when level finishes (default)
     - **OnGameEnd**: Save when game ends
     - **Never**: Don't save (temporary items)

### 2. Setup Level Data

1. Create SceneData asset (Right-click → Create → Scenes → Scene Data)
2. Configure for each level:
   - **Scene ID/Name**: Level identifier
   - **Level ID**: Unique identifier (e.g., "level_1")
   - **Collectible Stats**: Link to LevelCollectibleStats asset

### 3. Create Level Collectible Stats

1. Right-click → Create → Collectible System → Level Stats
2. Configure for each level:
   - **Level ID**: Must match SceneData's Level ID
   - **Collectible Stats List**: Define types and totals for this level

### 4. Setup Managers

Add these to your scene (should persist between scenes):
- **CollectibleManager**: Handles collecting and saving
- **LevelStatsManager**: Tracks level-specific stats
- **CollectibleStorage**: Stores all collectible types
- **CollectibleCounter** (optional): Counts collectibles in scene at startup

## Implementation

### Basic Usage

```csharp
// Initialize level stats when entering a level
CollectibleSystem.LevelStatsManager.Instance.InitializeStatsForLevel(sceneData);

// Subscribe to stats updates for UI
CollectibleSystem.LevelStatsManager.Instance.OnStatsUpdated += UpdateUI;

void UpdateUI(CollectibleSystem.LevelCollectibleStats stats)
{
    // Get runtime stats (collected/remaining counts)
    var runtimeStats = CollectibleSystem.LevelStatsManager.Instance.GetRuntimeStats();
    
    foreach (var stat in runtimeStats)
    {
        // Get the total count from the ScriptableObject (never changes)
        int total = CollectibleSystem.LevelStatsManager.Instance.GetTotalCount(stat.collectibleTypeId);
        
        Debug.Log("Collectible " + stat.collectibleTypeId + ": " +
                  stat.collectedCount + "/" + total + " collected");
    }
}

// Save progress when appropriate
public void OnLevelComplete()
{
    // Save level stats
    CollectibleSystem.LevelStatsManager.Instance.SaveCurrentLevelStats();
    
    // Save temporary items (those with SaveBehavior.OnLevelComplete)
    CollectibleSystem.CollectibleManager.Instance.SaveTemporaryItems();
}

public void OnGameSave()
{
    // Save all progress
    CollectibleSystem.CollectibleManager.Instance.SaveToPlayerPrefs();
    CollectibleSystem.LevelStatsManager.Instance.SaveCurrentLevelStats();
}
```

### Creating Collectibles

```csharp
public class Coin : MonoBehaviour, CollectibleSystem.ICollectibleCollector
{
    [SerializeField] private CollectibleSystem.CollectibleType coinType;
    [SerializeField] private int value = 1;
    [SerializeField] private string uniqueId; // Optional for one-time collectibles

    public void Collect(CollectibleSystem.Collectible collectible)
    {
        Debug.Log("Collected: " + collectible.Type.DisplayName);
        
        // Let the manager handle saving based on save behavior
        CollectibleSystem.CollectibleManager.Instance.Collect(collectible);
        
        // Custom logic
        if (collectible.Type.Id == "coin")
        {
            // Add to player inventory
        }
    }
}
```

### Unique Collectibles

For items that should only be collected once:

```csharp
[SerializeField] private string uniqueId = "special_sword_001";

void Awake()
{
    // Check if already collected at level start
    if (CollectibleSystem.CollectibleManager.Instance.IsUniqueIdCollected(uniqueId))
    {
        gameObject.SetActive(false);
    }
}

void OnTriggerEnter2D(Collider2D collision)
{
    var collector = collision.GetComponent<CollectibleSystem.ICollectibleCollector>();
    
    if (collector != null)
    {
        // Check if this specific unique ID has already been collected
        if (!string.IsNullOrEmpty(uniqueId) && 
            CollectibleSystem.CollectibleManager.Instance.IsUniqueIdCollected(uniqueId))
        {
            Debug.Log("This item was already collected: " + uniqueId);
            gameObject.SetActive(false);
            return;
        }
        
        collector.Collect(this);
    }
}
```

### UI Display

```csharp
public class CollectibleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI gemText;

    private void OnEnable()
    {
        CollectibleSystem.LevelStatsManager.Instance.OnStatsUpdated += UpdateStats;
    }

    private void OnDisable()
    {
        CollectibleSystem.LevelStatsManager.Instance.OnStatsUpdated -= UpdateStats;
    }

    private void UpdateStats(CollectibleSystem.LevelCollectibleStats stats)
    {
        var runtimeStats = CollectibleSystem.LevelStatsManager.Instance.GetRuntimeStats();
        
        foreach (var stat in runtimeStats)
        {
            int total = CollectibleSystem.LevelStatsManager.Instance.GetTotalCount(stat.collectibleTypeId);
            
            if (stat.collectibleTypeId == "coin")
            {
                coinText.text = "Coins: " + stat.collectedCount + "/" + total;
            }
            else if (stat.collectibleTypeId == "gem")
            {
                gemText.text = "Gems: " + stat.collectedCount + "/" + total;
            }
        }
    }
}
```

## Data Flow

1. **Initialization**: When entering a level, call `InitializeStatsForLevel()`
2. **Collection**: When collecting an item, the `CollectibleManager` handles saving based on the item's save behavior
3. **Runtime Updates**: `LevelStatsManager` updates runtime stats and notifies listeners
4. **Persistence**: Call `SaveCurrentLevelStats()` and `SaveTemporaryItems()` at appropriate times
5. **Loading**: Stats are automatically loaded from PlayerPrefs during initialization

## Important Notes

1. **ScriptableObject Integrity**: The total counts in `LevelCollectibleStats` are preserved and never modified at runtime
2. **Runtime vs Persistent Data**: 
   - `collectibleStats` in SO contains total counts (never changes)
   - `runtimeStats` contains collected/remaining counts (calculated at runtime)
   - Collected counts are saved to PlayerPrefs separately
3. **Save Behaviors**:
   - **OnCollection**: Saved immediately to permanent storage
   - **OnLevelComplete**: Saved to temporary storage until level completion
   - **OnGameEnd**: Saved to permanent storage when game ends
   - **Never**: Not saved at all
4. **Unique IDs**: Use for items that should only be collected once per save

## Best Practices

1. **Set totals in SO once**: Configure total counts in LevelCollectibleStats and never change them
2. **Use appropriate save behaviors**: Choose save behavior based on item importance
3. **Initialize on level load**: Always call InitializeStatsForLevel() when entering a level
4. **Save at appropriate times**: Save level stats on level complete, game save, or game end
5. **Test persistence**: Clear PlayerPrefs during development to test initial state
6. **Use events for UI**: Subscribe to OnStatsUpdated event for real-time UI updates
7. **Handle unique collectibles**: Use unique IDs for special items that should only be collected once

## Debugging

To reset all collectible data during development:

```csharp
PlayerPrefs.DeleteAll();
```

To clear specific level stats:

```csharp
CollectibleSystem.LevelStatsManager.Instance.ClearCurrentLevelStats();
```

To check saved data:

```csharp
Debug.Log("PlayerPrefs keys: " + string.Join(", ", PlayerPrefsUtil.GetAllKeys()));
```

(Note: You may need to create a PlayerPrefsUtil helper class for this)

## Summary

The CollectibleSystem provides a robust solution for tracking collectible items with:
- Clean separation between configuration (SO) and runtime data
- Flexible save behaviors for different item types
- Proper persistence across game sessions
- Support for unique collectibles
- Real-time stats tracking and UI updates

By following the setup and usage patterns described above, you can easily integrate collectible tracking into your game while maintaining clean code organization and proper data persistence.