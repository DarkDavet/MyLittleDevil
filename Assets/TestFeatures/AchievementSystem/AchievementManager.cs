using System;
using System.Collections.Generic;
using UnityEngine;

namespace AchievementSystem
{
    /// <summary>
    /// Manages achievement progress, registration, and persistence.
    /// This class is NOT a singleton — access it through AchievementSystemCore.Instance.AchievementManager.
    /// 
    /// Responsibilities:
    /// - Register AchievementType assets
    /// - Track progress and unlock status
    /// - Save/Load to PlayerPrefs
    /// - Raise events when achievements are unlocked or progress changes
    /// </summary>
    public class AchievementManager : MonoBehaviour
    {
        public delegate void AchievementUnlockedHandler(AchievementType achievementType);
        public event AchievementUnlockedHandler OnAchievementUnlocked;

        public delegate void AchievementProgressHandler(AchievementType achievementType, int currentProgress, int requiredProgress);
        public event AchievementProgressHandler OnAchievementProgress;

        private Dictionary<string, AchievementType> achievementTypes;
        private Dictionary<string, AchievementData> achievementProgress;
        private List<IAchievementListener> listeners = new List<IAchievementListener>();

        [System.Serializable]
        public class SaveData
        {
            public List<string> ids;
            public List<bool> unlocked;
            public List<int> progress;
        }

        /// <summary>
        /// Register all achievement types. Call this once during initialization.
        /// </summary>
        public void RegisterAchievementTypes(List<AchievementType> types)
        {
            achievementTypes = new Dictionary<string, AchievementType>();
            achievementProgress = new Dictionary<string, AchievementData>();

            foreach (var type in types)
            {
                if (!string.IsNullOrEmpty(type.Id))
                {
                    achievementTypes[type.Id] = type;
                    achievementProgress[type.Id] = new AchievementData(type);
                }
            }
            LoadFromPlayerPrefs();
        }

        /// <summary>
        /// Register a single achievement type.
        /// </summary>
        public void RegisterAchievementType(AchievementType type)
        {
            if (achievementTypes == null) achievementTypes = new Dictionary<string, AchievementType>();
            if (achievementProgress == null) achievementProgress = new Dictionary<string, AchievementData>();

            if (type != null && !string.IsNullOrEmpty(type.Id))
            {
                achievementTypes[type.Id] = type;
                if (!achievementProgress.ContainsKey(type.Id))
                {
                    achievementProgress[type.Id] = new AchievementData(type);
                }
            }
        }

        public void AddListener(IAchievementListener listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        public void RemoveListener(IAchievementListener listener)
        {
            listeners.Remove(listener);
        }

        /// <summary>
        /// Update progress for an achievement. Automatically unlocks if required progress is reached.
        /// </summary>
        public void UpdateProgress(string achievementId, int amount = 1)
        {
            if (achievementProgress == null || !achievementProgress.ContainsKey(achievementId))
                return;

            AchievementData data = achievementProgress[achievementId];
            if (data.isUnlocked)
                return;

            AchievementType type = achievementTypes[achievementId];
            data.currentProgress = Mathf.Clamp(data.currentProgress + amount, 0, type.RequiredProgress);

            OnAchievementProgress?.Invoke(type, data.currentProgress, type.RequiredProgress);

            foreach (var listener in listeners)
                listener.OnAchievementProgress(type, data.currentProgress, type.RequiredProgress);

            if (data.currentProgress >= type.RequiredProgress)
            {
                UnlockAchievement(achievementId);
            }
            else
            {
                // Сохраняем промежуточный прогресс
                SaveToPlayerPrefs();
            }
        }

        /// <summary>
        /// Instantly unlock an achievement.
        /// </summary>
        public void UnlockAchievement(string achievementId)
        {
            if (achievementProgress == null || !achievementProgress.ContainsKey(achievementId))
                return;

            AchievementData data = achievementProgress[achievementId];
            if (data.isUnlocked) return;

            AchievementType type = achievementTypes[achievementId];
            data.isUnlocked = true;
            data.currentProgress = type.RequiredProgress;

            // 1. Оповещаем слушателей (включая панель)
            OnAchievementUnlocked?.Invoke(type);
            foreach (var listener in listeners)
                listener.OnAchievementUnlocked(type);

            // 2. ВАЖНО: Вызываем визуальное уведомление через Core
            AchievementSystemCore.Instance.ShowNotification(type);

            // 3. Сохраняем результат
            SaveToPlayerPrefs();
        }

        public bool IsAchievementUnlocked(string achievementId)
        {
            if (achievementProgress == null || !achievementProgress.TryGetValue(achievementId, out var data))
                return false;
            return data.isUnlocked;
        }

        public int GetAchievementProgress(string achievementId)
        {
            if (achievementProgress == null || !achievementProgress.TryGetValue(achievementId, out var data))
                return 0;
            return data.currentProgress;
        }

        public int GetTotalUnlockedCount()
        {
            if (achievementProgress == null) return 0;
            int count = 0;
            foreach (var data in achievementProgress.Values)
            {
                if (data.isUnlocked) count++;
            }
            return count;
        }

        public int GetTotalAchievementCount()
        {
            return achievementProgress == null ? 0 : achievementProgress.Count;
        }

        public List<AchievementData> GetAllAchievementData()
        {
            return achievementProgress == null ? new List<AchievementData>() : new List<AchievementData>(achievementProgress.Values);
        }

        public Dictionary<string, AchievementType> GetAchievementTypes()
        {
            return achievementTypes;
        }

        public Dictionary<string, AchievementData> GetAchievementProgress()
        {
            return achievementProgress;
        }

        public void SaveToPlayerPrefs()
        {
            if (achievementProgress == null || achievementProgress.Count == 0) return;

            var data = new SaveData
            {
                ids = new List<string>(),
                unlocked = new List<bool>(),
                progress = new List<int>()
            };

            foreach (var entry in achievementProgress)
            {
                data.ids.Add(entry.Key);
                data.unlocked.Add(entry.Value.isUnlocked);
                data.progress.Add(entry.Value.currentProgress);
            }

            PlayerPrefs.SetString("Achievements", JsonUtility.ToJson(data));
        }

        public void LoadFromPlayerPrefs()
        {
            if (achievementProgress == null) return;

            if (PlayerPrefs.HasKey("Achievements"))
            {
                string json = PlayerPrefs.GetString("Achievements");
                SaveData data = JsonUtility.FromJson<SaveData>(json);

                if (data != null)
                {
                    for (int i = 0; i < data.ids.Count; i++)
                    {
                        if (achievementProgress.ContainsKey(data.ids[i]))
                        {
                            AchievementData entry = achievementProgress[data.ids[i]];
                            entry.isUnlocked = data.unlocked[i];
                            entry.currentProgress = data.progress[i];
                        }
                    }
                }
            }
        }

        public void ResetProgress()
        {
            if (achievementProgress == null) return;

            foreach (var entry in achievementProgress)
            {
                entry.Value.isUnlocked = false;
                entry.Value.currentProgress = 0;
            }
            SaveToPlayerPrefs();
        }
    }
}