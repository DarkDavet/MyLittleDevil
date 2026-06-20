using AchievementSystem.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AchievementSystem
{
    public class AchievementSystemCore : MonoBehaviour
    {
        [Header("Achievement database")]
        [SerializeField] private AchievementDatabase achievDatabase;

        [Header("UI References")]
        [Tooltip("Prefab for AchievementNotificationUI (optional, can be null if UI is separate)")]
        [SerializeField] private GameObject notificationPrefab;

        [Header("Canvas Settings")]
        [Tooltip("Automatically create a persistent Canvas for notifications")]
        [SerializeField] private bool autoCreatePersistentCanvas = true;

        [Tooltip("Resolution for the notification Canvas")]
        [SerializeField] private Vector2 canvasResolution = new Vector2(1920, 1080);

        [Tooltip("Match width or height for Canvas scaling")]
        [SerializeField] private bool canvasMatchWidthOrHeight = true;

        private static AchievementSystemCore _instance;
        public static AchievementSystemCore Instance => _instance;

        private AchievementManager _achievementManager;
        public AchievementManager AchievementManager => _achievementManager;

        private Transform _persistentCanvasRoot;
        private Canvas _notificationCanvas;

        private void Awake()
        {
            // Singleton pattern with DontDestroyOnLoad
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                
                // Create persistent notification canvas
                if (autoCreatePersistentCanvas)
                {
                    CreatePersistentNotificationCanvas();
                }
                
                InitializeSystem();
            }
            else
            {
                if (_instance != this)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void InitializeSystem()
        {
            if (_achievementManager == null) // Если мы еще не создавали его в этом сеансе
            {
                var managerObj = new GameObject("AchievementManager");
                _achievementManager = managerObj.AddComponent<AchievementManager>();
                managerObj.transform.SetParent(this.transform); // Привязываем к Core, чтобы они жили вместе
            }

            // Register all achievement types
            if (achievDatabase.allAchievements != null && achievDatabase.allAchievements.Count > 0)
            {
                _achievementManager.RegisterAchievementTypes(achievDatabase.allAchievements);
                Debug.Log($"[AchievementSystemCore] Registered {achievDatabase.allAchievements.Count} achievements.");
            }
            else
            {
                Debug.LogWarning("[AchievementSystemCore] No achievements assigned! Assign AchievementType assets to the All Achievements array.");
            }
        }

        // ==========================================
        // Convenient wrapper methods for common operations
        // ==========================================

        /// <summary>
        /// Update unique progress for an achievement. Automatically unlocks if required progress is reached.
        /// </summary>
        public void UpdateUniqueProgress(string achievementId, string uniqueId)
        {
            _achievementManager?.AddUniqueProgress(achievementId, uniqueId);
        }

        /// <summary>
        /// Update progress for an achievement. Automatically unlocks if required progress is reached.
        /// </summary>
        public void UpdateStandartProgress(string achievementId, int amount = 1)
        {
            _achievementManager?.AddProgress(achievementId, amount);
        }

        public void ResetAchievementProgress(string achievementId)
        {
            _achievementManager?.ResetAchievementProgress(achievementId);
        }

        public void DecreaseAchievementProgress(string achievementId, int amount = 1)
        {
            _achievementManager?.DecreaseProgress(achievementId, amount);
        }

        /// <summary>
        /// Instantly unlock an achievement.
        /// </summary>
        public void UnlockAchievement(string achievementId)
        {
            _achievementManager?.UnlockAchievement(achievementId);
        }

        /// <summary>
        /// Check if an achievement is unlocked.
        /// </summary>
        public bool IsAchievementUnlocked(string achievementId)
        {
            return _achievementManager?.IsAchievementUnlocked(achievementId) ?? false;
        }

        /// <summary>
        /// Get current progress for an achievement.
        /// </summary>
        public int GetAchievementProgress(string achievementId)
        {
            return _achievementManager?.GetAchievementProgress(achievementId) ?? 0;
        }

        /// <summary>
        /// Get total number of unlocked achievements.
        /// </summary>
        public int GetTotalUnlockedCount()
        {
            return _achievementManager?.GetTotalUnlockedCount() ?? 0;
        }

        /// <summary>
        /// Get total number of registered achievements.
        /// </summary>
        public int GetTotalAchievementCount()
        {
            return _achievementManager?.GetTotalAchievementCount() ?? 0;
        }

        /// <summary>
        /// Show notification UI for an unlocked achievement.
        /// Uses the persistent Canvas root as parent.
        /// </summary>
        public void ShowNotification(AchievementType achievementType)
        {
            if (notificationPrefab == null)
            {
                Debug.LogError("Notification Prefab не назначен в AchievementSystemCore!");
                return;
            }

            if (_persistentCanvasRoot == null) CreatePersistentNotificationCanvas();

            // Спавним в корень со столбиком
            GameObject notificationObj = Instantiate(notificationPrefab, _persistentCanvasRoot);

            RectTransform rt = notificationObj.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.anchoredPosition = Vector2.zero;
            }

            notificationObj.GetComponent<AchievementNotificationUI>()?.Show(achievementType);

            Destroy(notificationObj, 5f);
        }

        private void CreatePersistentNotificationCanvas()
        {
            GameObject canvasObj = new GameObject("AchievementNotificationCanvas");
            DontDestroyOnLoad(canvasObj);

            _notificationCanvas = canvasObj.AddComponent<Canvas>();
            _notificationCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _notificationCanvas.sortingOrder = 999;

            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = canvasResolution;
            scaler.matchWidthOrHeight = canvasMatchWidthOrHeight ? 1f : 0f;

            canvasObj.AddComponent<GraphicRaycaster>();

            GameObject rootObj = new GameObject("NotificationRoot");
            rootObj.transform.SetParent(canvasObj.transform, false);

            RectTransform rt = rootObj.AddComponent<RectTransform>();
            rt.anchorMin = new Vector2(1, 1); 
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(1, 1);
            rt.anchoredPosition = new Vector2(-20, -20); 
            rt.sizeDelta = new Vector2(400, 0); 

            ContentSizeFitter fitter = rootObj.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            VerticalLayoutGroup layout = rootObj.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 10; 
            layout.childAlignment = TextAnchor.UpperRight; 
            layout.childControlHeight = false; 
            layout.childControlWidth = false; 
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = false;

            _persistentCanvasRoot = rootObj.transform;
        }

        /// <summary>
        /// Get all registered achievement types.
        /// </summary>
        public List<AchievementType> GetAllAchievements()
        {
            return achievDatabase.allAchievements;
        }

        /// <summary>
        /// Save all achievement progress to PlayerPrefs.
        /// </summary>
        public void SaveProgress()
        {
            _achievementManager?.SaveToPlayerPrefs();
        }

        /// <summary>
        /// Load all achievement progress from PlayerPrefs.
        /// </summary>
        public void LoadProgress()
        {
            _achievementManager?.LoadFromPlayerPrefs();
        }

        /// <summary>
        /// Reset all achievement progress.
        /// </summary>
        public void ResetProgress()
        {
            _achievementManager?.ResetProgress();
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}