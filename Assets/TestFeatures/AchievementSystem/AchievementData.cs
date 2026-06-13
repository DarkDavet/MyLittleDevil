using System.Collections.Generic;
using UnityEngine;

namespace AchievementSystem
{
    [System.Serializable]
    public class AchievementData
    {
        public string achievementId;
        public bool isUnlocked;

        public List<string> earnedUniqueIds;

        public int currentProgress => earnedUniqueIds?.Count ?? 0;

        public AchievementData(AchievementType achievementType)
        {
            achievementId = achievementType.Id;
            isUnlocked = false;
            earnedUniqueIds = new List<string>();
        }

        public AchievementData()
        {
            achievementId = string.Empty;
            isUnlocked = false;
            earnedUniqueIds = new List<string>();
        }
    }
}