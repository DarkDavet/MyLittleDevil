using System;
using System.Collections.Generic;
using System.Xml;
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
            public List<string> serializedIds;
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
        /// Увеличивает прогресс на определенное количество (для монет, обычных врагов и т.д.)
        /// </summary>
        public void AddProgress(string achievementId, int amount = 1)
        {
            if (achievementProgress == null || !achievementProgress.ContainsKey(achievementId))
                return;

            AchievementData data = achievementProgress[achievementId];
            if (data.isUnlocked)
                return;

            if (data.earnedUniqueIds == null)
                data.earnedUniqueIds = new List<string>();

            AchievementType type = achievementTypes[achievementId];

            // Вычисляем, сколько еще осталось до максимума, чтобы не выйти за лимит
            int spaceLeft = type.RequiredProgress - data.currentProgress;
            int itemsToAdd = Mathf.Clamp(amount, 0, spaceLeft);

            // Просто добавляем нужное количество случайных ID, чтобы увеличить count
            for (int i = 0; i < itemsToAdd; i++)
            {
                data.earnedUniqueIds.Add(System.Guid.NewGuid().ToString());
            }

            // Оповещаем шкалу и слушателей
            OnAchievementProgress?.Invoke(type, data.currentProgress, type.RequiredProgress);

            foreach (var listener in listeners)
                listener.OnAchievementProgress(type, data.currentProgress, type.RequiredProgress);

            if (data.currentProgress >= type.RequiredProgress)
            {
                UnlockAchievement(achievementId);
            }
            else
            {
                SaveToPlayerPrefs();
            }
        }

        /// <summary>
        /// Добавляет прогресс только если переданный ID объекта уникален и еще не засчитывался.
        /// </summary>
        public void AddUniqueProgress(string achievementId, string uniqueId)
        {
            if (achievementProgress == null || !achievementProgress.ContainsKey(achievementId))
                return;

            AchievementData data = achievementProgress[achievementId];
            if (data.isUnlocked)
                return;

            if (data.earnedUniqueIds == null)
                data.earnedUniqueIds = new List<string>();

            // ГЛАВНАЯ ЗАЩИТА: Если этот ID уровня/босса уже есть, ничего не делаем
            if (data.earnedUniqueIds.Contains(uniqueId))
                return;

            // Добавляем уникальный идентификатор
            data.earnedUniqueIds.Add(uniqueId);

            AchievementType type = achievementTypes[achievementId];

            // Оповещаем шкалу и слушателей (data.currentProgress автоматически вернет размер списка)
            OnAchievementProgress?.Invoke(type, data.currentProgress, type.RequiredProgress);

            foreach (var listener in listeners)
                listener.OnAchievementProgress(type, data.currentProgress, type.RequiredProgress);

            if (data.currentProgress >= type.RequiredProgress)
            {
                UnlockAchievement(achievementId);
            }
            else
            {
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

            // 1. Оповещаем слушателей (включая панель)
            OnAchievementUnlocked?.Invoke(type);
            foreach (var listener in listeners)
                listener.OnAchievementUnlocked(type);

            // 2. ВАЖНО: Вызываем визуальное уведомление через Core
            AchievementSystemCore.Instance.ShowNotification(type);

            // 3. Сохраняем результат
            SaveToPlayerPrefs();
        }

        /// <summary>
        /// Сбрасывает прогресс конкретной ачивки по её ID.
        /// </summary>
        public void ResetAchievementProgress(string achievementId)
        {
            if (achievementProgress == null || !achievementProgress.ContainsKey(achievementId))
                return;

            AchievementData data = achievementProgress[achievementId];

            // Если ачивка уже получена, сбрасывать её прогресс нельзя
            if (data.isUnlocked)
                return;

            AchievementType type = achievementTypes[achievementId];

            // Очищаем накопленные ID
            if (data.earnedUniqueIds != null)
                data.earnedUniqueIds.Clear();
            else
                data.earnedUniqueIds = new List<string>();

            // Оповещаем UI и слушателей, что прогресс теперь обнулился
            OnAchievementProgress?.Invoke(type, 0, type.RequiredProgress);

            foreach (var listener in listeners)
                listener.OnAchievementProgress(type, 0, type.RequiredProgress);

            SaveToPlayerPrefs();
        }

        /// <summary>
        /// Уменьшает прогресс конкретной ачивки на указанное количество, если она еще не разблокирована.
        /// </summary>
        public void DecreaseProgress(string achievementId, int amount = 1)
        {
            if (achievementProgress == null || !achievementProgress.ContainsKey(achievementId))
                return;

            AchievementData data = achievementProgress[achievementId];

            // Защита: если ачивка уже получена, уменьшать прогресс нельзя
            if (data.isUnlocked)
                return;

            // Если уменьшать нечего, просто выходим
            if (data.earnedUniqueIds == null || data.earnedUniqueIds.Count == 0 || amount <= 0)
                return;

            // Вычисляем, сколько элементов реально нужно удалить (не больше, чем есть в списке)
            int itemsToRemove = Mathf.Min(amount, data.earnedUniqueIds.Count);

            // Удаляем элементы с конца списка
            for (int i = 0; i < itemsToRemove; i++)
            {
                data.earnedUniqueIds.RemoveAt(data.earnedUniqueIds.Count - 1);
            }

            AchievementType type = achievementTypes[achievementId];

            // Оповещаем UI и слушателей о новом уменьшенном прогрессе
            OnAchievementProgress?.Invoke(type, data.currentProgress, type.RequiredProgress);

            foreach (var listener in listeners)
                listener.OnAchievementProgress(type, data.currentProgress, type.RequiredProgress);

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
                serializedIds = new List<string>()
            };

            foreach (var entry in achievementProgress)
            {
                data.ids.Add(entry.Key);
                data.unlocked.Add(entry.Value.isUnlocked);

                // Превращаем список ["Level_1", "Level_2"] в строку "Level_1;Level_2"
                string joinedIds = entry.Value.earnedUniqueIds != null
                    ? string.Join(";", entry.Value.earnedUniqueIds)
                    : string.Empty;

                data.serializedIds.Add(joinedIds);
            }

            PlayerPrefs.SetString("Achievements", JsonUtility.ToJson(data));
            PlayerPrefs.Save();
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
                            entry.earnedUniqueIds = new List<string>();

                            // Проверяем, были ли сохранены ID
                            if (data.serializedIds != null && i < data.serializedIds.Count && !string.IsNullOrEmpty(data.serializedIds[i]))
                            {
                                // Разрезаем строку "Level_1;Level_2" обратно в список элементов
                                string[] splitIds = data.serializedIds[i].Split(';');
                                entry.earnedUniqueIds.AddRange(splitIds);
                            }
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
                if (entry.Value.earnedUniqueIds != null)
                    entry.Value.earnedUniqueIds.Clear();
                else
                    entry.Value.earnedUniqueIds = new List<string>();
            }
            SaveToPlayerPrefs();
        }
    }
}