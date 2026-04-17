using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine;

namespace CollectibleSystem
{
    [CreateAssetMenu(fileName = "NewLevelCollectibleStats", menuName = "Collectible System/Level Stats")]
    public class LevelCollectibleStats : ScriptableObject
    {
        [System.Serializable]
        public class CollectibleTypeStats
        {
            public string collectibleTypeId;
            public int totalCount; // This value is preserved in SO and never changed
        }

        // Runtime data for tracking collected/remaining counts
        [System.NonSerialized]
        public List<RuntimeCollectibleStats> runtimeStats = new List<RuntimeCollectibleStats>();

        [System.Serializable]
        public class RuntimeCollectibleStats
        {
            public int totalCount;
            public string collectibleTypeId;
            public int collectedCount;
            public int remainingCount;
        }

        public string levelId;
        public List<CollectibleTypeStats> collectibleStats;

        [System.Serializable]
        public class LevelCollectibleStatsData
        {
            public string levelId;
            public List<CollectibleTypeStatsDataEntry> collectibleStats;
        }

        [System.Serializable]
        public class CollectibleTypeStatsDataEntry
        {
            public string collectibleTypeId;
            public int totalCount;
            public int collectedCount;
            public int remainingCount;
        }

        private Dictionary<string, RuntimeCollectibleStats> runtimeDict = new Dictionary<string, RuntimeCollectibleStats>();

        public void CalculateRemainingCounts()
        {
            runtimeStats.Clear();
            runtimeDict.Clear();
            foreach (var stat in collectibleStats)
            {
                var runtime = new RuntimeCollectibleStats
                {
                    collectibleTypeId = stat.collectibleTypeId,
                    totalCount = stat.totalCount 
                };
                runtimeStats.Add(runtime);
                runtimeDict[stat.collectibleTypeId] = runtime;
            }
        }

        public int GetCollectedCount(string collectibleTypeId)
        {
            foreach (var stat in runtimeStats)
            {
                if (stat.collectibleTypeId == collectibleTypeId)
                {
                    return stat.collectedCount;
                }
            }
            return 0;
        }

        public int GetRemainingCount(string collectibleTypeId)
        {
            foreach (var stat in runtimeStats)
            {
                if (stat.collectibleTypeId == collectibleTypeId)
                {
                    return stat.remainingCount;
                }
            }
            return 0;
        }

        public void IncrementCollectedCount(string collectibleTypeId, int amount)
        {
            int total = GetTotalCount(collectibleTypeId);
            
            foreach (var stat in runtimeStats)
            {
                if (stat.collectibleTypeId == collectibleTypeId)
                {
                    int newCollected = stat.collectedCount + amount;
                    // Don't exceed total count
                    stat.collectedCount = Mathf.Min(newCollected, total);
                    stat.remainingCount = total - stat.collectedCount;
                    return;
                }
            }
            
            // If not found, only add if it exists in collectibleStats
            if (HasCollectibleType(collectibleTypeId))
            {
                int finalCollected = Mathf.Min(amount, total);
                runtimeStats.Add(new RuntimeCollectibleStats
                {
                    collectibleTypeId = collectibleTypeId,
                    collectedCount = finalCollected,
                    remainingCount = total - finalCollected
                });
            }
        }

        public int GetTotalCount(string collectibleTypeId)
        {
            foreach (var stat in collectibleStats)
            {
                if (stat.collectibleTypeId == collectibleTypeId)
                {
                    return stat.totalCount;
                }
            }
            return 0;
        }

        public bool HasCollectibleType(string collectibleTypeId)
        {
            foreach (var stat in collectibleStats)
            {
                if (stat.collectibleTypeId == collectibleTypeId)
                {
                    return true;
                }
            }
            return false;
        }

        // Save/Load methods for persisting collected counts
        public void SaveLevelStats(string saveKey)
        {
            string json = JsonUtility.ToJson(new LevelCollectibleStatsData
            {
                levelId = levelId,
                collectibleStats = runtimeStats.ConvertAll(stat => new CollectibleTypeStatsDataEntry
                {
                    collectibleTypeId = stat.collectibleTypeId,
                    totalCount = GetTotalCount(stat.collectibleTypeId),
                    collectedCount = stat.collectedCount,
                    remainingCount = stat.remainingCount
                }
                )
            });
            PlayerPrefs.SetString(saveKey, json);
            PlayerPrefs.Save();
        }

        public void LoadLevelStats(string saveKey)
        {
            if (PlayerPrefs.HasKey(saveKey))
            {
                string json = PlayerPrefs.GetString(saveKey);
                LevelCollectibleStatsData data = JsonUtility.FromJson<LevelCollectibleStatsData>(json);

                Debug.Log("Loading level stats for " + levelId + " from save key: " + saveKey);
                Debug.Log("  Found " + data.collectibleStats.Count + " collectible types");
                
                runtimeStats.Clear();
                foreach (var savedStat in data.collectibleStats)
                {
                    runtimeStats.Add(new RuntimeCollectibleStats
                    {
                        collectibleTypeId = savedStat.collectibleTypeId,
                        collectedCount = savedStat.collectedCount,
                        remainingCount = savedStat.remainingCount
                    });
                    Debug.Log("    " + savedStat.collectibleTypeId + 
                              ": Collected=" + savedStat.collectedCount + 
                              ", Remaining=" + savedStat.remainingCount);
                }
            }
            else
            {
                Debug.Log("No saved data found for " + levelId + " (save key: " + saveKey + ")");
            }
        }

        public void DisplayLevelProgress(string levelId, LevelCollectibleStats levelStatsAsset)
        {
            string saveKey = "LevelStats_" + levelId;

            // Временный объект для загрузки данных без активации рантайма
            if (PlayerPrefs.HasKey(saveKey))
            {
                string json = PlayerPrefs.GetString(saveKey);
                var data = JsonUtility.FromJson<LevelCollectibleStatsData>(json);

                foreach (var entry in data.collectibleStats)
                {
                    // Здесь спавним виджеты (например, 6/10) в ячейку уровня
                    Debug.Log($"Уровень {levelId}: {entry.collectibleTypeId} {entry.collectedCount}/{entry.totalCount}");
                }
            }
        }

        public void ClearLevelStats(string saveKey)
        {
            if (PlayerPrefs.HasKey(saveKey))
            {
                PlayerPrefs.DeleteKey(saveKey);
                PlayerPrefs.Save();
            }
        }
    }
}