using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AchievementSystem.UI
{
    public class AchievementPanelUI : MonoBehaviour, IAchievementListener
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Transform slotsContainer;
        [SerializeField] private TextMeshProUGUI totalCountText;
        [SerializeField] private TextMeshProUGUI unlockedCountText;
        [SerializeField] private Button closeButton;

        private List<AchievementSlotUI> slotUIs = new List<AchievementSlotUI>();
        private List<AchievementData> allData = new List<AchievementData>();
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (closeButton != null)
                closeButton.onClick.AddListener(Close);
        }

        private void Start()
        {
            // Use AchievementSystemCore.Instance as the single entry point
            AchievementSystemCore.Instance?.AchievementManager?.AddListener(this);
            
            // Auto-initialize slots from all registered achievements
            InitializeSlots();
        }
        
        /// <summary>
        /// Auto-initialize all achievement slots from AchievementSystemCore.
        /// Called automatically on Start.
        /// </summary>
        private void InitializeSlots()
        {
            if (AchievementSystemCore.Instance == null)
            {
                Debug.LogWarning("[AchievementPanelUI] AchievementSystemCore.Instance is null!");
                return;
            }

            List<AchievementType> achievementTypes = AchievementSystemCore.Instance.GetAllAchievements();
            if (achievementTypes == null || achievementTypes.Count == 0)
            {
                Debug.LogWarning("[AchievementPanelUI] No achievements registered in AchievementSystemCore!");
                return;
            }

            allData = AchievementSystemCore.Instance.AchievementManager.GetAllAchievementData();
            
            foreach (var type in achievementTypes)
            {
                AchievementData data = null;
                foreach (var d in allData)
                {
                    if (d.achievementId == type.Id)
                    {
                        data = d;
                        break;
                    }
                }

                if (data == null) continue;

                GameObject slotObj = Instantiate(slotPrefab, slotsContainer);
                AchievementSlotUI slotUI = slotObj.GetComponent<AchievementSlotUI>();
                if (slotUI != null)
                {
                    slotUI.Setup(type, data);
                    slotUIs.Add(slotUI);
                }
            }

            UpdateCounts();
        }

        private void OnDestroy()
        {
            AchievementSystemCore.Instance?.AchievementManager?.RemoveListener(this);
        }

        /// <summary>
        /// Show panel with achievement slots.
        /// If achievementTypes is null, uses all registered achievements from AchievementSystemCore.
        /// </summary>
        public void Show(List<AchievementType> achievementTypes = null)
        {
            // Clear existing slots
            foreach (var slot in slotUIs)
                Destroy(slot.gameObject);
            slotUIs.Clear();

            // If no achievement types provided, use all from AchievementSystemCore
            if (achievementTypes == null)
            {
                achievementTypes = AchievementSystemCore.Instance?.GetAllAchievements();
            }

            if (achievementTypes == null || achievementTypes.Count == 0)
            {
                Debug.LogWarning("[AchievementPanelUI] No achievements to display!");
                return;
            }

            allData = AchievementSystemCore.Instance?.AchievementManager?.GetAllAchievementData() ?? new List<AchievementData>();
            
            foreach (var type in achievementTypes)
            {
                AchievementData data = null;
                foreach (var d in allData)
                {
                    if (d.achievementId == type.Id)
                    {
                        data = d;
                        break;
                    }
                }

                if (data == null) continue;

                GameObject slotObj = Instantiate(slotPrefab, slotsContainer);
                AchievementSlotUI slotUI = slotObj.GetComponent<AchievementSlotUI>();
                if (slotUI != null)
                {
                    slotUI.Setup(type, data);
                    slotUIs.Add(slotUI);
                }
            }

            UpdateCounts();

            // Show panel with fade-in
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.DOFade(1, 0.25f);
        }

        public void Close()
        {
            _canvasGroup.DOFade(0, 0.25f).OnComplete(() =>
            {
                _canvasGroup.blocksRaycasts = false;
                gameObject.SetActive(false);
            });
        }

        public void UpdateCounts()
        {
            var manager = AchievementSystemCore.Instance?.AchievementManager;
            if (manager == null) return;

            if (totalCountText != null)
                totalCountText.text = $"{manager.GetTotalAchievementCount()}";

            if (unlockedCountText != null)
                unlockedCountText.text = $"{manager.GetTotalUnlockedCount()}";
        }

        public void OnAchievementUnlocked(AchievementType achievementType)
        {
            UpdateCounts();

            foreach (var slot in slotUIs)
            {
                if (slot.GetComponent<AchievementSlotUI>() != null)
                {
                    // Refresh the slot to show unlocked state
                    foreach (var data in allData)
                    {
                        if (data.achievementId == achievementType.Id)
                        {
                            slot.Setup(achievementType, data);
                            break;
                        }
                    }
                }
            }
        }

        public void OnAchievementProgress(AchievementType achievementType, int currentProgress, int requiredProgress)
        {
            UpdateCounts();

            foreach (var slot in slotUIs)
            {
                AchievementSlotUI slotUI = slot.GetComponent<AchievementSlotUI>();
                if (slotUI != null)
                {
                    slotUI.UpdateProgress(currentProgress, requiredProgress);
                }
            }
        }
    }
}