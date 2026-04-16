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
                currentLevelStats = sceneData.collectibleStats;
                string saveKey = "LevelStats_" + sceneData.sceneID;
                
                currentLevelStats.LoadLevelStats(saveKey);
                
                // If no saved data, initialize with zero collected counts
                if (currentLevelStats.runtimeStats.Count == 0)
                {
                    currentLevelStats.runtimeStats.Clear();
                    
                    foreach (var stat in currentLevelStats.collectibleStats)
                    {
                        // Get count of items already collected (saved in CollectibleManager)
                        int alreadyCollected = CollectibleManager.Instance.GetItemCount(stat.collectibleTypeId);
                        int remaining = stat.totalCount - alreadyCollected;
                        
                        currentLevelStats.runtimeStats.Add(new LevelCollectibleStats.RuntimeCollectibleStats
                        {
                            collectibleTypeId = stat.collectibleTypeId,
                            collectedCount = alreadyCollected,
                            remainingCount = remaining
                        });
                    }
                }
                
                OnStatsUpdated?.Invoke(currentLevelStats);
            }
        }

        public LevelCollectibleStats GetCurrentStats()
        {
            return currentLevelStats;
        }

        public List<LevelCollectibleStats.RuntimeCollectibleStats> GetRuntimeStats()
        {
            if (currentLevelStats != null)
            {
                return currentLevelStats.runtimeStats;
            }
            return new List<LevelCollectibleStats.RuntimeCollectibleStats>();
        }

        public int GetTotalCount(string collectibleTypeId)
        {
            if (currentLevelStats != null)
            {
                return currentLevelStats.GetTotalCount(collectibleTypeId);
            }
            return 0;
        }

        private List<string> GetAllPlayerPrefsKeys()
        {
            List<string> keys = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                string key = PlayerPrefs.GetString("PlayerPrefsKeys" + i);
                if (string.IsNullOrEmpty(key))
                    break;
                keys.Add(key);
            }
            return keys;
        }
        
        public void UpdateStats(string collectibleTypeId, int amount)
        {
            if (currentLevelStats == null) return;
            
            // Get current collected count and total
            int currentCollected = currentLevelStats.GetCollectedCount(collectibleTypeId);
            int total = currentLevelStats.GetTotalCount(collectibleTypeId);
            
            // Calculate new collected count, but don't exceed total
            int newCollected = currentCollected + amount;
            int finalCollected = Mathf.Min(newCollected, total);
            
            // Update the collected count directly
            foreach (var stat in currentLevelStats.runtimeStats)
            {
                if (stat.collectibleTypeId == collectibleTypeId)
                {
                    stat.collectedCount = finalCollected;
                    stat.remainingCount = total - finalCollected;
                    break;
                }
            }
            OnStatsUpdated?.Invoke(currentLevelStats);
        }

         public void SaveCurrentLevelStats()
         {
             if (currentLevelStats != null && currentLevelStats.levelId != null)
             {
                 string saveKey = "LevelStats_" + currentLevelStats.levelId;
                 currentLevelStats.SaveLevelStats(saveKey);
                 Debug.Log("Saved level stats for: " + currentLevelStats.levelId);
             }
         }

         public void ClearCurrentLevelStats()
         {
             if (currentLevelStats != null && currentLevelStats.levelId != null)
             {
                 string saveKey = "LevelStats_" + currentLevelStats.levelId;
                 currentLevelStats.ClearLevelStats(saveKey);
                 Debug.Log("Cleared level stats for: " + currentLevelStats.levelId);
             }
         }
    }
}