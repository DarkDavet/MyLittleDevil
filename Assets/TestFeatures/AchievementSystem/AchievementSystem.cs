using AchievementSystem.UI;
using System.Collections.Generic;
using UnityEngine;

namespace AchievementSystem
{
    public class AchievementSystem : MonoBehaviour
    {
        [SerializeField] private List<AchievementType> achievements;
        [SerializeField] private GameObject notificationPrefab;
        [SerializeField] private Transform notificationContainer;

        private AchievementManager achievementManager;
        private Dictionary<string, AchievementType> achievementDict;

        private void Awake()
        {
            achievementManager = FindObjectOfType<AchievementManager>();
            if (achievementManager == null)
            {
                Debug.LogError("AchievementManager not found in scene!");
                enabled = false;
                return;
            }

            if (achievements != null)
            {
                achievementManager.RegisterAchievementTypes(achievements);
                achievementDict = new Dictionary<string, AchievementType>();
                foreach (var achievement in achievements)
                {
                    if (!string.IsNullOrEmpty(achievement.Id))
                        achievementDict[achievement.Id] = achievement;
                }
            }

            achievementManager.OnAchievementUnlocked += OnAchievementUnlocked;
        }

        private void OnDestroy()
        {
            achievementManager.OnAchievementUnlocked -= OnAchievementUnlocked;
        }

        private void OnAchievementUnlocked(AchievementType achievementType)
        {
            ShowNotification(achievementType);
        }

        private void ShowNotification(AchievementType achievementType)
        {
            if (notificationPrefab == null) return;

            GameObject notificationObj = Instantiate(notificationPrefab, notificationContainer);
            AchievementNotificationUI notification = notificationObj.GetComponent<AchievementNotificationUI>();
            if (notification != null)
            {
                notification.Show(achievementType);
            }
        }

        public void UnlockAchievement(string achievementId)
        {
            achievementManager?.UnlockAchievement(achievementId);
        }

        public void UpdateAchievementProgress(string achievementId, int amount = 1)
        {
            achievementManager?.UpdateProgress(achievementId, amount);
        }

        public bool IsAchievementUnlocked(string achievementId)
        {
            return achievementManager?.IsAchievementUnlocked(achievementId) ?? false;
        }

        public List<AchievementType> GetAllAchievements()
        {
            return achievements;
        }
    }
}