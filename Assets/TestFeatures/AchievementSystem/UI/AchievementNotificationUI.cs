using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AchievementSystem.UI
{
    public class AchievementNotificationUI : MonoBehaviour
    {
        [SerializeField] private Image notificationIcon;
        [SerializeField] private TextMeshProUGUI notificationTitle;
        [SerializeField] private TextMeshProUGUI notificationDescription;
        [SerializeField] private TextMeshProUGUI rewardText;
        [SerializeField] private CanvasGroup canvasGroup;

        private Tween fadeTween;
        private float displayDuration = 3f;
        private float fadeDuration = 0.5f;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }

        public void Show(AchievementType achievementType)
        {
            notificationIcon.sprite = achievementType.UnlockedIcon != null
                ? achievementType.UnlockedIcon
                : achievementType.Icon;
            notificationTitle.text = achievementType.Title;
            notificationDescription.text = achievementType.UnlockMessage != null
                ? achievementType.UnlockMessage
                : achievementType.Description;

            if (achievementType.RewardCoins > 0)
            {
                rewardText.text = $"+{achievementType.RewardCoins} Coins";
                rewardText.gameObject.SetActive(true);
            }
            else
            {
                rewardText.gameObject.SetActive(false);
            }

            ShowNotification();
        }

        private void ShowNotification()
        {
            canvasGroup.blocksRaycasts = true;
            fadeTween?.Kill();

            canvasGroup.DOFade(1, fadeDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    StartCoroutine(DelayedHide());
                });
        }

        private IEnumerator DelayedHide()
        {
            yield return new WaitForSeconds(displayDuration);
            HideNotification();
        }

        private void HideNotification()
        {
            fadeTween = canvasGroup.DOFade(0, fadeDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    canvasGroup.blocksRaycasts = false;
                    gameObject.SetActive(false);
                });
        }

        public void OnClick()
        {
            if (fadeTween != null && fadeTween.IsActive() && !fadeTween.IsComplete())
            {
                fadeTween.Complete();
                HideNotification();
            }
        }
    }
}