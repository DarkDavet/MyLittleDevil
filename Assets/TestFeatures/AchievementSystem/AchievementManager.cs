using System;
using System.Collections.Generic;
using UnityEngine;

namespace AchievementSystem
{
    public class AchievementManager : MonoBehaviour
    {
        public static AchievementManager Instance { get; private set; }

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

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            LoadFromPlayerPrefs();
        }

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

        public void UpdateProgress(string achievementId, int amount = 1)
        {
            if (!achievementProgress.ContainsKey(achievementId))
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
                UnlockAchievement(achievementId);
        }

        public void UnlockAchievement(string achievementId)
        {
            if (!achievementProgress.ContainsKey(achievementId))
                return;

            AchievementData data = achievementProgress[achievementId];
            if (data.isUnlocked)
                return;

            AchievementType type = achievementTypes[achievementId];
            data.isUnlocked = true;
            data.currentProgress = type.RequiredProgress;

            OnAchievementUnlocked?.Invoke(type);

            foreach (var listener in listeners)
                listener.OnAchievementUnlocked(type);

            SaveToPlayerPrefs();
        }

        public bool IsAchievementUnlocked(string achievementId)
        {
            if (achievementProgress.TryGetValue(achievementId, out var data))
                return data.isUnlocked;
            return false;
        }

        public int GetAchievementProgress(string achievementId)
        {
            if (achievementProgress.TryGetValue(achievementId, out var data))
                return data.currentProgress;
            return 0;
        }

        public int GetTotalUnlockedCount()
        {
            int count = 0;
            foreach (var data in achievementProgress.Values)
            {
                if (data.isUnlocked) count++;
            }
            return count;
        }

        public int GetTotalAchievementCount()
        {
            return achievementProgress.Count;
        }

        public List<AchievementData> GetAllAchievementData()
        {
            return new List<AchievementData>(achievementProgress.Values);
        }

        public void SaveToPlayerPrefs()
        {
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
            foreach (var entry in achievementProgress)
            {
                entry.Value.isUnlocked = false;
                entry.Value.currentProgress = 0;
            }
            SaveToPlayerPrefs();
        }
    }
}