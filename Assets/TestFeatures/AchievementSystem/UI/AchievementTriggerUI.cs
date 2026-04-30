using UnityEngine;
using UnityEngine.EventSystems;

namespace AchievementSystem.UI
{
    public class AchievementTriggerUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private string achievementId;
        [SerializeField] private int progressAmount = 1;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!string.IsNullOrEmpty(achievementId))
            {
                AchievementManager.Instance?.UpdateProgress(achievementId, progressAmount);
            }
        }
    }
}