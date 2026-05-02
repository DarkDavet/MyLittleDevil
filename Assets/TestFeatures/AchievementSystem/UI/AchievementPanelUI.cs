using DG.Tweening;
using System.Collections;
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
        private CanvasGroup _canvasGroup;
        private bool _isInitialized = false;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (closeButton != null)
                closeButton.onClick.AddListener(Close);

            // Гарантируем, что панель невидима при старте сцены
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 0;
                _canvasGroup.blocksRaycasts = false;
            }
        }

        private void Start()
        {
            // Подписываемся на события менеджера
            AchievementSystemCore.Instance?.AchievementManager?.AddListener(this);

            // Предварительно создаем слоты, если менеджер уже готов
            if (!_isInitialized)
            {
                InitializeSlots();
            }
        }

        private void OnEnable()
        {
            // Каждый раз при активации объекта (Show) обновляем данные
            UpdateCounts();
            RefreshExistingSlots();
        }

        private void InitializeSlots()
        {
            if (AchievementSystemCore.Instance == null || AchievementSystemCore.Instance.AchievementManager == null)
                return;

            var types = AchievementSystemCore.Instance.GetAllAchievements();
            var progressDict = AchievementSystemCore.Instance.AchievementManager.GetAchievementProgress();

            // Очищаем контейнер от старых объектов (если есть)
            foreach (Transform child in slotsContainer) Destroy(child.gameObject);
            slotUIs.Clear();

            foreach (var type in types)
            {
                if (progressDict.TryGetValue(type.Id, out var data))
                {
                    GameObject slotObj = Instantiate(slotPrefab, slotsContainer);
                    AchievementSlotUI slotUI = slotObj.GetComponent<AchievementSlotUI>();
                    if (slotUI != null)
                    {
                        slotUI.Setup(type, data);
                        slotUIs.Add(slotUI);
                    }
                }
            }
            _isInitialized = true;
        }

        private void RefreshExistingSlots()
        {
            if (AchievementSystemCore.Instance == null) return;

            var manager = AchievementSystemCore.Instance.AchievementManager;
            var progressDict = manager.GetAchievementProgress();
            var typesDict = manager.GetAchievementTypes();

            foreach (var slot in slotUIs)
            {
                if (progressDict.TryGetValue(slot.AchievementId, out var data))
                {
                    slot.Setup(typesDict[slot.AchievementId], data);
                }
            }
        }

        /// <summary>
        /// Главный метод для вызова открытия окна
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true); // Это триггерит OnEnable и Refresh

            if (!_isInitialized) InitializeSlots();

            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.DOFade(1, 0.25f).SetUpdate(true);
        }

        public void Close()
        {
            _canvasGroup.DOFade(0, 0.25f).SetUpdate(true).OnComplete(() =>
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
                totalCountText.text = manager.GetTotalAchievementCount().ToString();

            if (unlockedCountText != null)
                unlockedCountText.text = manager.GetTotalUnlockedCount().ToString();
        }

        // Слушатели событий
        public void OnAchievementUnlocked(AchievementType achievementType)
        {
            UpdateCounts();
            UpdateSpecificSlot(achievementType.Id);
        }

        public void OnAchievementProgress(AchievementType achievementType, int currentProgress, int requiredProgress)
        {
            UpdateCounts();
            UpdateSpecificSlot(achievementType.Id);
        }

        private void UpdateSpecificSlot(string id)
        {
            foreach (var slot in slotUIs)
            {
                if (slot.AchievementId == id)
                {
                    var manager = AchievementSystemCore.Instance.AchievementManager;
                    slot.Setup(manager.GetAchievementTypes()[id], manager.GetAchievementProgress()[id]);
                    break;
                }
            }
        }

        private void OnDestroy()
        {
            AchievementSystemCore.Instance?.AchievementManager?.RemoveListener(this);
        }
    }
}