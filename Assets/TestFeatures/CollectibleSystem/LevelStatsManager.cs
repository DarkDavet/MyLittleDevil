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
                
                // Calculate remaining counts based on what's already collected
                foreach (var stat in currentLevelStats.collectibleStats)
                {
                    stat.collectedCount = CollectibleManager.Instance.GetItemCount(stat.collectibleTypeId);
                    stat.remainingCount = stat.totalCount - stat.collectedCount;
                }
                
                OnStatsUpdated?.Invoke(currentLevelStats);
            }
        }
        
        public LevelCollectibleStats GetCurrentStats()
        {
            return currentLevelStats;
        }
        
        public void UpdateStats(string collectibleTypeId, int amount)
        {
            if (currentLevelStats == null) return;
            
            foreach (var stat in currentLevelStats.collectibleStats)
            {
                if (stat.collectibleTypeId == collectibleTypeId)
                {
                    stat.collectedCount += amount;
                    stat.remainingCount = stat.totalCount - stat.collectedCount;
                    break;
                }
            }
            
            OnStatsUpdated?.Invoke(currentLevelStats);
        }
    }
}