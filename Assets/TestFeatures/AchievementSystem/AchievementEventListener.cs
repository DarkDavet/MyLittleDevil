using System.Collections.Generic;
using UnityEngine;

namespace AchievementSystem
{
    /// <summary>
    /// Attach this component to any GameObject in the scene to automatically
    /// update achievement progress when specific events occur.
    /// 
    /// Simply drag AchievementType assets into the appropriate event arrays
    /// in the Inspector — no need to type IDs manually.
    /// </summary>
    public class AchievementEventListener : MonoBehaviour
    {
        [Header("Event Triggers — Drag AchievementType assets here")]
        
        [Tooltip("Achievements to update when an enemy is killed")]
        [SerializeField] private List<AchievementType> onKillAchievements = new List<AchievementType>();

        [Tooltip("Achievements to update when an item is collected")]
        [SerializeField] private List<AchievementType> onCollectAchievements = new List<AchievementType>();

        [Tooltip("Achievements to update when damage is dealt")]
        [SerializeField] private List<AchievementType> onDamageDealtAchievements = new List<AchievementType>();

        [Tooltip("Achievements to update when player heals")]
        [SerializeField] private List<AchievementType> onHealedAchievements = new List<AchievementType>();

        [Tooltip("Achievements to update when distance is traveled")]
        [SerializeField] private List<AchievementType> onDistanceTraveledAchievements = new List<AchievementType>();

        [Tooltip("Achievements to update when a dialogue finishes")]
        [SerializeField] private List<AchievementType> onDialogueFinishedAchievements = new List<AchievementType>();

        [Tooltip("Achievements to update when a level is completed")]
        [SerializeField] private List<AchievementType> onLevelCompleteAchievements = new List<AchievementType>();

        [Tooltip("Achievements to update when a chest is opened")]
        [SerializeField] private List<AchievementType> onChestOpenedAchievements = new List<AchievementType>();

        private void OnEnable()
        {
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
                    AchievementManager.Instance?.UpdateProgress(achievement.Id, 1);
                }
            }
        }
    }
}