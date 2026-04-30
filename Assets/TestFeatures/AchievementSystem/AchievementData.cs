using UnityEngine;

namespace AchievementSystem
{
    [System.Serializable]
    public class AchievementData
    {
        public string achievementId;
        public bool isUnlocked;
        public int currentProgress;

        public AchievementData(AchievementType achievementType)
        {
            achievementId = achievementType.Id;
            isUnlocked = false;
            currentProgress = 0;
        }

        public AchievementData()
        {
            achievementId = string.Empty;
            isUnlocked = false;
            currentProgress = 0;
        }
    }
}