# AchievementCondition Usage Guide

## What is AchievementCondition?

`AchievementCondition` is a MonoBehaviour component you **attach to GameObjects in the Unity Editor**. When certain in-game events happen (player collects an item, kills an enemy, travels distance, etc.), the condition automatically notifies the `AchievementManager` to update progress.

## How It Works — Simple Flow

```
1. You create an AchievementType asset (e.g., "Collect 10 coins")
       ↓
2. You attach a CollectibleCondition component to a GameObject in your scene
       ↓
3. You set the Achievement ID field to match the achievement ("collect_10_coins")
       ↓
4. When the player collects an item → CollectibleCondition detects it → calls NotifyProgress()
       ↓
5. AchievementManager receives the update → checks if progress reached target → unlocks if yes
```

## Step-by-Step Example: "Collect 10 Coins"

### Step 1: Create the Achievement Type

1. In Unity Project window: **Right-click** → **Create** → **Achievement System** → **Achievement**
2. Rename to `FirstTenCoins`
3. In Inspector:
   - **ID**: `collect_10_coins`
   - **Title**: `Coin Collector`
   - **Description**: `Collect 10 coins`
   - **Required Progress**: `10`
   - **Progress Type**: `Counter`

### Step 2: Create a Trigger GameObject

1. In Hierarchy: **Right-click** → **Create Empty**
2. Rename to `CoinCollector`
3. Add a **BoxCollider2D** component:
   - Set **Is Trigger** = ✓
   - Adjust size to cover your coin spawn area

### Step 3: Add the Condition Component

1. Select `CoinCollector` GameObject
2. In Inspector: **Add Component** → `CollectibleCondition`
3. In the CollectibleCondition inspector:
   - **Achievement ID**: `collect_10_coins` (must match the AchievementType ID exactly)
   - **Progress Per Action**: `1` (how much progress each collection gives)

### Step 4: Test

When your player collects a coin while inside the BoxCollider2D area, the achievement progress increases by 1. After 10 coins, the achievement unlocks automatically.

## Available Condition Types

| Component | Use Case | Setup |
|---|---|---|
| `CollectibleCondition` | Player picks up an item | Set Achievement ID, ensure Collider2D + IsTrigger |
| `KillCondition` | Player defeats an enemy | Set Achievement ID, set Enemy Tag (default: "Enemy") |
| `DistanceCondition` | Player travels distance | Set Achievement ID, set Distance Per Unit |
| `TimeCondition` | Player spends time playing | Set Achievement ID, set Time Per Action (in seconds) |
| `DialogueCondition` | Player finishes a dialogue | Attach to GameObject with DialogueSystem component |

## Manual Trigger (No Condition Component Needed)

If you don't want to use condition components, you can trigger achievements from **any script**:

```csharp
using AchievementSystem;

public class MyCustomScript : MonoBehaviour
{
    private void SomeEvent()
    {
        // Increment progress by 1
        AchievementManager.Instance.UpdateProgress("collect_10_coins", 1);
        
        // Or unlock immediately (for binary achievements)
        AchievementManager.Instance.UnlockAchievement("defeat_boss");
    }
}
```

## Common Mistakes

| Problem | Solution |
|---|---|
| Achievement doesn't unlock | Check that Achievement ID in condition matches the AchievementType ID exactly (case-sensitive) |
| AchievementManager is null | Make sure an AchievementManager GameObject exists in the scene |
| Progress not updating | Check Console for errors — the condition may not be detecting collisions |
| Achievement unlocks too fast | Increase Required Progress in the AchievementType asset |

## Quick Reference

```
// In Unity Editor:
AchievementType asset → Set ID = "my_achievement"
GameObject → Add CollectibleCondition → Set Achievement ID = "my_achievement"

// In Code:
AchievementManager.Instance.UpdateProgress("my_achievement", 1);  // Add progress
AchievementManager.Instance.UnlockAchievement("my_achievement");  // Unlock now
AchievementManager.Instance.IsAchievementUnlocked("my_achievement");  // Check status