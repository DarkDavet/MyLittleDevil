using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AchievementSystem.UI
{
    public class AchievementSlotUI : MonoBehaviour
    {
        public string AchievementId => achievementType != null ? achievementType.Id : string.Empty;

        [SerializeField] private Image icon;
        [SerializeField] private Image iconBackground;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Image progressFill;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private GameObject lockedOverlay;
        [SerializeField] private GameObject unlockedOverlay;
        [SerializeField] private GameObject hiddenOverlay;

        private AchievementType achievementType;
        private AchievementData achievementData;

        public void Setup(AchievementType type, AchievementData data)
        {
            achievementType = type;
            achievementData = data;

            if (type.IsHidden && !data.isUnlocked)
            {
                icon.sprite = null;
                title.text = "???";
                description.text = "Keep playing to discover...";
                iconBackground.color = Color.gray;

                if (hiddenOverlay != null) hiddenOverlay.SetActive(true);
                if (lockedOverlay != null) lockedOverlay.SetActive(true);
                if (unlockedOverlay != null) unlockedOverlay.SetActive(false);
                if (progressFill != null) progressFill.gameObject.SetActive(false);
                if (progressText != null) progressText.gameObject.SetActive(false);
            }
            else if (data.isUnlocked)
            {
                icon.sprite = type.UnlockedIcon != null ? type.UnlockedIcon : type.Icon;
                title.text = type.Title;
                description.text = type.UnlockMessage != null ? type.UnlockMessage : type.Description;
                iconBackground.color = Color.white;

                if (hiddenOverlay != null) hiddenOverlay.SetActive(false);
                if (lockedOverlay != null) lockedOverlay.SetActive(false);
                if (unlockedOverlay != null) unlockedOverlay.SetActive(true);
                if (progressFill != null) progressFill.gameObject.SetActive(false);
                if (progressText != null)
                {
                    progressText.gameObject.SetActive(true);
                    progressText.text = "Completed!";
                }
            }
            else
            {
                icon.sprite = type.Icon;
                title.text = type.Title;
                description.text = type.Description;
                iconBackground.color = Color.white;

                if (hiddenOverlay != null) hiddenOverlay.SetActive(false);
                if (lockedOverlay != null) lockedOverlay.SetActive(true);
                if (unlockedOverlay != null) unlockedOverlay.SetActive(false);

                if (progressFill != null)
                {
                    progressFill.gameObject.SetActive(true);
                    float progress = type.RequiredProgress > 0
                        ? (float)data.currentProgress / type.RequiredProgress
                        : 0f;
                    progressFill.fillAmount = progress;
                }

                if (progressText != null)
                {
                    progressText.gameObject.SetActive(true);
                    progressText.text = $"{data.currentProgress}/{type.RequiredProgress}";
                }
            }
        }

        public void UpdateProgress(int currentProgress, int requiredProgress)
        {
            if (achievementData == null || achievementType == null) return;
            if (achievementData.isUnlocked) return;

            achievementData.currentProgress = currentProgress;

            if (progressFill != null)
            {
                float progress = requiredProgress > 0
                    ? (float)currentProgress / requiredProgress
                    : 0f;
                progressFill.fillAmount = progress;
            }

            if (progressText != null)
            {
                progressText.text = $"{currentProgress}/{requiredProgress}";
            }
        }
    }
}