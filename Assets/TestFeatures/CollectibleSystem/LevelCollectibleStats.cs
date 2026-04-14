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
            public int totalCount;
            [NonSerialized] public int collectedCount;
            [NonSerialized] public int remainingCount;
        }

        public string levelId;
        public List<CollectibleTypeStats> collectibleStats;

        public void CalculateRemainingCounts()
        {
            foreach (var stat in collectibleStats)
            {
                stat.remainingCount = stat.totalCount - stat.collectedCount;
            }
        }
    }
}