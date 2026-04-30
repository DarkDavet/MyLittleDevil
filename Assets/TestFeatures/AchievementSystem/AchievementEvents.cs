using UnityEngine;
using UnityEngine.Events;

namespace AchievementSystem
{
    /// <summary>
    /// Static event bus for achievement-related events.
    /// Call these from any game system to trigger achievement progress checks.
    /// </summary>
    public static class AchievementEvents
    {
        /// <summary>
        /// Fired when player kills an enemy.
        /// </summary>
        public static event System.Action OnKill;

        /// <summary>
        /// Fired when player collects an item.
        /// </summary>
        public static event System.Action<CollectibleSystem.Collectible> OnCollect;

        /// <summary>
        /// Fired when player deals damage to an enemy.
        /// </summary>
        public static event System.Action<float> OnDamageDealt;

        /// <summary>
        /// Fired when player heals.
        /// </summary>
        public static event System.Action<int> OnHealed;

        /// <summary>
        /// Fired when player travels a distance.
        /// </summary>
        public static event System.Action<float> OnDistanceTraveled;

        /// <summary>
        /// Fired when a dialogue finishes.
        /// </summary>
        public static event System.Action<string> OnDialogueFinished;

        /// <summary>
        /// Fired when player completes a level.
        /// </summary>
        public static event System.Action<string> OnLevelComplete;

        /// <summary>
        /// Fired when player opens a chest/lootable object.
        /// </summary>
        public static event System.Action<string> OnChestOpened;

        /// <summary>
        /// Triggered when an enemy is killed.
        /// Call from enemy death logic.
        /// </summary>
        public static void TriggerKill()
        {
            OnKill?.Invoke();
        }

        /// <summary>
        /// Triggered when an item is collected.
        /// Call from item collection logic.
        /// </summary>
        public static void TriggerCollect(CollectibleSystem.Collectible collectible)
        {
            OnCollect?.Invoke(collectible);
        }

        /// <summary>
        /// Triggered when player deals damage.
        /// Call from damage dealing logic.
        /// </summary>
        public static void TriggerDamageDealt(float damage)
        {
            OnDamageDealt?.Invoke(damage);
        }

        /// <summary>
        /// Triggered when player heals.
        /// Call from healing logic.
        /// </summary>
        public static void TriggerHealed(int amount)
        {
            OnHealed?.Invoke(amount);
        }

        /// <summary>
        /// Triggered when player travels distance.
        /// Call from movement tracking logic.
        /// </summary>
        public static void TriggerDistanceTraveled(float distance)
        {
            OnDistanceTraveled?.Invoke(distance);
        }

        /// <summary>
        /// Triggered when a dialogue finishes.
        /// Call from dialogue system.
        /// </summary>
        public static void TriggerDialogueFinished(string dialogueId)
        {
            OnDialogueFinished?.Invoke(dialogueId);
        }

        /// <summary>
        /// Triggered when a level is completed.
        /// Call from level completion logic.
        /// </summary>
        public static void TriggerLevelComplete(string levelId)
        {
            OnLevelComplete?.Invoke(levelId);
        }

        /// <summary>
        /// Triggered when a chest is opened.
        /// Call from chest/lootable object logic.
        /// </summary>
        public static void TriggerChestOpened(string chestId)
        {
            OnChestOpened?.Invoke(chestId);
        }
    }

    /// <summary>
    /// Unity-compatible events for use in Unity Editor (Inspector).
    /// Attach AchievementEventListener to GameObjects to subscribe.
    /// </summary>
    [System.Serializable]
    public class AchievementUnityEvents
    {
        public UnityEvent onKill = new UnityEvent();
        public UnityEvent onCollect = new UnityEvent();
        public UnityEvent onDamageDealt = new UnityEvent();
        public UnityEvent onHealed = new UnityEvent();
        public UnityEvent onDistanceTraveled = new UnityEvent();
        public UnityEvent onDialogueFinished = new UnityEvent();
        public UnityEvent onLevelComplete = new UnityEvent();
        public UnityEvent onChestOpened = new UnityEvent();
    }
}