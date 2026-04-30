using UnityEngine;

namespace AchievementSystem
{
    public enum AchievementCategory
    {
        Collection,
        Combat,
        Exploration,
        Customization,
        Special
    }

    public enum AchievementProgressType
    {
        Binary,      // Unlocked or not (no progress)
        Counter,     // Numeric progress (e.g., collect 100 coins)
        Percentage   // Percentage-based progress (0-100)
    }

    [CreateAssetMenu(fileName = "NewAchievement", menuName = "Achievement System/Achievement")]
    public class AchievementType : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private Sprite unlockedIcon;
        [SerializeField] private AchievementCategory category;
        [SerializeField] private AchievementProgressType progressType;
        [SerializeField] private int requiredProgress = 1;
        [SerializeField] private int rewardCoins;
        [SerializeField] private bool isHidden = false;
        [SerializeField] private string unlockMessage;

        public string Id => id;
        public string Title => title;
        public string Description => description;
        public Sprite Icon => icon;
        public Sprite UnlockedIcon => unlockedIcon;
        public AchievementCategory Category => category;
        public AchievementProgressType ProgressType => progressType;
        public int RequiredProgress => requiredProgress;
        public int RewardCoins => rewardCoins;
        public bool IsHidden => isHidden;
        public string UnlockMessage => unlockMessage;
    }
}