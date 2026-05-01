using System.Collections.Generic;
using UnityEngine;

namespace AchievementSystem
{
    /// <summary>
    /// Attach this component to any GameObject to automatically update achievement progress
    /// when specific events occur in the game.
    /// 
    /// Simply drag AchievementType assets into the appropriate event arrays in the Inspector.
    /// Each event type corresponds to a specific game action:
    /// - On Kill: achievements for killing enemies
    /// - On Collect: achievements for collecting items
    /// - On Damage Dealt: achievements for dealing damage
    /// - On Healed: achievements for healing
    /// - On Distance Traveled: achievements for traveling distance
    /// - On Dialogue Finished: achievements for finishing dialogues
    /// - On Level Complete: achievements for completing levels
    /// - On Chest Opened: achievements for opening chests
    /// </summary>
    public class AchievementEventListener : MonoBehaviour
    {
        [Header("Event Triggers — Drag AchievementType assets here")]
        
        [SerializeField] private List<AchievementType> onKillAchievements = new List<AchievementType>();
        [SerializeField] private List<AchievementType> onCollectAchievements = new List<AchievementType>();
        [SerializeField] private List<AchievementType> onDamageDealtAchievements = new List<AchievementType>();
        [SerializeField] private List<AchievementType> onHealedAchievements = new List<AchievementType>();
        [SerializeField] private List<AchievementType> onDistanceTraveledAchievements = new List<AchievementType>();
        [SerializeField] private List<AchievementType> onDialogueFinishedAchievements = new List<AchievementType>();
        [SerializeField] private List<AchievementType> onLevelCompleteAchievements = new List<AchievementType>();
        [SerializeField] private List<AchievementType> onChestOpenedAchievements = new List<AchievementType>();

        private void OnEnable()
        {
            // Subscribe to events
            AchievementEvents.OnKill += OnKill;
            AchievementEvents.OnCollect += OnCollect;
            AchievementEvents.OnDamageDealt += OnDamageDealt;
            AchievementEvents.OnHealed += OnHealed;
            AchievementEvents.OnDistanceTraveled += OnDistanceTraveled;
            AchievementEvents.OnDialogueFinished += OnDialogueFinished;
            AchievementEvents.OnLevelComplete += OnLevelComplete;
            AchievementEvents.OnChestOpened += OnChestOpened;
        }

        private void OnDisable()
        {
            // Unsubscribe from events
            AchievementEvents.OnKill -= OnKill;
            AchievementEvents.OnCollect -= OnCollect;
            AchievementEvents.OnDamageDealt -= OnDamageDealt;
            AchievementEvents.OnHealed -= OnHealed;
            AchievementEvents.OnDistanceTraveled -= OnDistanceTraveled;
            AchievementEvents.OnDialogueFinished -= OnDialogueFinished;
            AchievementEvents.OnLevelComplete -= OnLevelComplete;
            AchievementEvents.OnChestOpened -= OnChestOpened;
        }

        private void OnKill()
        {
            ProcessAchievements(onKillAchievements);
        }

        private void OnCollect(CollectibleSystem.Collectible collectible)
        {
            ProcessAchievements(onCollectAchievements);
        }

        private void OnDamageDealt(float damage)
        {
            ProcessAchievements(onDamageDealtAchievements);
        }

        private void OnHealed(int amount)
        {
            ProcessAchievements(onHealedAchievements);
        }

        private void OnDistanceTraveled(float distance)
        {
            ProcessAchievements(onDistanceTraveledAchievements);
        }

        private void OnDialogueFinished(string dialogueId)
        {
            ProcessAchievements(onDialogueFinishedAchievements);
        }

        private void OnLevelComplete(string levelId)
        {
            ProcessAchievements(onLevelCompleteAchievements);
        }

        private void OnChestOpened(string chestId)
        {
            ProcessAchievements(onChestOpenedAchievements);
        }

        private void ProcessAchievements(List<AchievementType> achievements)
        {
            if (achievements == null || achievements.Count == 0) return;

            foreach (var achievement in achievements)
            {
                if (achievement != null && !string.IsNullOrEmpty(achievement.Id))
                {
                    // Use AchievementSystemCore.Instance as the single entry point
                    AchievementSystemCore.Instance?.UpdateProgress(achievement.Id, 1);
                }
            }
        }
    }
}