using AchievementSystem;
using System.Collections.Generic;
using UnityEngine;

namespace CollectibleSystem
{
    public class LevelStatsManager : MonoBehaviour
    {
        public static LevelStatsManager Instance { get; private set; }

        public delegate void StatsUpdatedHandler(LevelCollectibleStats stats);
        public event StatsUpdatedHandler OnStatsUpdated;

        private LevelCollectibleStats currentLevelStats;
        private Dictionary<string, int> statsAtStartOfSession = new Dictionary<string, int>();

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
        }

        public void InitializeStatsForLevel(SceneData sceneData)
        {
            if (currentLevelStats != null)
            {
                RollbackStatsOnDefeat();
            }

            if (sceneData != null && sceneData.collectibleStats != null)
            {
                currentLevelStats = sceneData.collectibleStats;
                string saveKey = "LevelStats_" + sceneData.sceneID;

                currentLevelStats.LoadLevelStats(saveKey);

                OnStatsUpdated?.Invoke(currentLevelStats);
            }

            statsAtStartOfSession.Clear();
            if (currentLevelStats != null && currentLevelStats.runtimeStats != null)
            {
                foreach (var stat in currentLevelStats.runtimeStats)
                {
                    statsAtStartOfSession[stat.collectibleTypeId] = stat.collectedCount;
                }
            }
        }

        public int GetAddedThisSession(string typeId)
        {
            if (currentLevelStats == null) return 0;
            int current = currentLevelStats.GetCollectedCount(typeId);
            int start = statsAtStartOfSession.ContainsKey(typeId) ? statsAtStartOfSession[typeId] : 0;
            return current - start;
        }

        public void UpdateStats(string collectibleTypeId, int amount)
        {
            if (currentLevelStats == null) return;

            // Централизованно обновляем рантайм-счетчики в SO
            currentLevelStats.IncrementCollectedCount(collectibleTypeId, amount);

            OnStatsUpdated?.Invoke(currentLevelStats);
        }

        public void SaveCurrentLevelStats()
        {
            if (currentLevelStats != null && !string.IsNullOrEmpty(currentLevelStats.levelId))
            {
                string saveKey = "LevelStats_" + currentLevelStats.levelId;
                currentLevelStats.SaveLevelStats(saveKey);
            }
        }

        // Хелперы для UI
        public LevelCollectibleStats GetCurrentStats() => currentLevelStats;

        public int GetTotalCount(string collectibleTypeId) => currentLevelStats?.GetTotalCount(collectibleTypeId) ?? 0;

        public void ClearCurrentLevelStats()
        {
            if (currentLevelStats != null)
            {
                string saveKey = "LevelStats_" + currentLevelStats.levelId;
                currentLevelStats.ClearLevelStats(saveKey);
                currentLevelStats.runtimeStats.Clear();
            }
        }

        public void CheckLevelClearAchievements()
        {
            if (currentLevelStats == null) return;

            if (currentLevelStats.IsLevelFullyCleared())
            {
                string lvlId = currentLevelStats.levelId;
                Debug.Log($"[Achievement SUCCESS] Уровень {lvlId} полностью зачищен на 100%!");

                AchievementSystemCore.Instance?.UnlockAchievement("clear_1_level");

                AchievementSystemCore.Instance?.UpdateUniqueProgress("clear_all_levels", lvlId);
            }
        }

        public void RollbackStatsOnDefeat()
        {
            if (currentLevelStats != null && !string.IsNullOrEmpty(currentLevelStats.levelId))
            {
                string saveKey = "LevelStats_" + currentLevelStats.levelId;

                currentLevelStats.LoadLevelStats(saveKey);
                OnStatsUpdated?.Invoke(currentLevelStats);
                Debug.Log($"[Stats] Статистика уровня {currentLevelStats.levelId} успешно откатана назад.");
            }
        }
    }
}