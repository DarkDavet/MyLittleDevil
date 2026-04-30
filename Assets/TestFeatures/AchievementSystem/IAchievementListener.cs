namespace AchievementSystem
{
    public interface IAchievementListener
    {
        void OnAchievementUnlocked(AchievementType achievementType);
        void OnAchievementProgress(AchievementType achievementType, int currentProgress, int requiredProgress);
    }
}