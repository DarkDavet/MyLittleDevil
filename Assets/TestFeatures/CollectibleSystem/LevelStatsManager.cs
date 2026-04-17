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
            if (sceneData != null && sceneData.collectibleStats != null)
            {
                // 1. Привязываем SO статистики к менеджеру
                currentLevelStats = sceneData.collectibleStats;
                // Используем sceneID для ключа сохранения
                string saveKey = "LevelStats_" + sceneData.sceneID;

                // 2. Пытаемся загрузить сохраненный прогресс именно этого уровня
                currentLevelStats.LoadLevelStats(saveKey);

                // 3. Если уровень запущен впервые (runtimeStats пустые после загрузки)
                if (currentLevelStats.runtimeStats.Count == 0)
                {
                    // Просто инициализируем пустые значения на основе списка collectibleStats в SO
                    currentLevelStats.CalculateRemainingCounts();
                }

                OnStatsUpdated?.Invoke(currentLevelStats);
            }

            statsAtStartOfSession.Clear();
            foreach (var stat in currentLevelStats.runtimeStats)
            {
                statsAtStartOfSession[stat.collectibleTypeId] = stat.collectedCount;
            }
        }

        public int GetAddedThisSession(string typeId)
        {
            int current = currentLevelStats.GetCollectedCount(typeId);
            int start = statsAtStartOfSession.ContainsKey(typeId) ? statsAtStartOfSession[typeId] : 0;
            return current - start;
        }

        public void UpdateStats(string collectibleTypeId, int amount)
        {
            if (currentLevelStats == null) return;

            // Используем внутренний метод SO для обновления — это централизует логику
            currentLevelStats.IncrementCollectedCount(collectibleTypeId, amount);

            OnStatsUpdated?.Invoke(currentLevelStats);
        }

        public void SaveCurrentLevelStats()
        {
            // Используем ID уровня из самого SO, чтобы не зависеть от внешних ссылок при сохранении
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
                currentLevelStats.runtimeStats.Clear(); // Обнуляем в рантайме
            }
        }
    }
}