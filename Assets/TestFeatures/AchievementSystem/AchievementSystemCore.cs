using AchievementSystem.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AchievementSystem
{
    /// <summary>
    /// Global singleton that manages the entire Achievement System across all scenes.
    /// This should be placed in a persistent scene (e.g., MainMenu) with DontDestroyOnLoad.
    ///
    /// This is the ONLY singleton in the Achievement System — all access goes through here.
    ///
    /// Responsibilities:
    /// - Initialize AchievementManager with all AchievementTypes
    /// - Provide convenient access to achievement methods
    /// - Handle notification UI spawning with persistent Canvas
    ///
    /// Setup:
    /// 1. Create empty GameObject named "AchievementSystemCore"
    /// 2. Add this component
    /// 3. Assign all AchievementType assets to the All Achievements array
    /// 4. (Optional) Assign AchievementNotificationUI prefab to Notification Prefab field
    /// 5. Place this GameObject in your first scene (MainMenu)
    ///
    /// Persistent Notification Canvas:
    /// - Automatically creates a dedicated Canvas for notifications with DontDestroyOnLoad
    /// - Notifications will ALWAYS appear on screen regardless of scene changes
    /// - Configurable resolution and match mode in Canvas Settings
    ///
    /// Usage:
    /// AchievementSystemCore.Instance.UpdateProgress("kill_10_enemies", 1);
    /// AchievementSystemCore.Instance.UnlockAchievement("complete_level_1");
    /// AchievementSystemCore.Instance.IsAchievementUnlocked("kill_10_enemies");
    /// </summary>
    public class AchievementSystemCore : MonoBehaviour
    {
        [Header("Achievement Configuration")]
        [Tooltip("All AchievementType ScriptableObjects in the game")]
        [SerializeField] private List<AchievementType> allAchievements = new List<AchievementType>();

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
            // Find existing AchievementManager or create one
            _achievementManager = FindObjectOfType<AchievementManager>();
            if (_achievementManager == null)
            {
                var managerObj = new GameObject("AchievementManager");
                _achievementManager = managerObj.AddComponent<AchievementManager>();
                DontDestroyOnLoad(managerObj);
            }

            // Register all achievement types
            if (allAchievements != null && allAchievements.Count > 0)
            {
                _achievementManager.RegisterAchievementTypes(allAchievements);
                Debug.Log($"[AchievementSystemCore] Registered {allAchievements.Count} achievements.");
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
        /// Update progress for an achievement. Automatically unlocks if required progress is reached.
        /// </summary>
        public void UpdateProgress(string achievementId, int amount = 1)
        {
            _achievementManager?.UpdateProgress(achievementId, amount);
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
                Debug.LogWarning("[AchievementSystemCore] No notification prefab assigned!");
                return;
            }

            // Use persistent canvas root as parent
            Transform parent = _persistentCanvasRoot;
            if (parent == null)
            {
                Debug.LogWarning("[AchievementSystemCore] Persistent notification canvas root not available!");
                return;
            }

            GameObject notificationObj = Instantiate(notificationPrefab, parent);
            AchievementNotificationUI notification = notificationObj.GetComponent<AchievementNotificationUI>();
            if (notification != null)
            {
                notification.Show(achievementType);
            }
            else
            {
                Debug.LogWarning("[AchievementSystemCore] Notification prefab missing AchievementNotificationUI component!");
                Destroy(notificationObj);
            }
        }

        /// <summary>
        /// Create a persistent Canvas for notification UI with DontDestroyOnLoad.
        /// This ensures notifications always appear on screen regardless of scene changes.
        /// </summary>
        private void CreatePersistentNotificationCanvas()
        {
            GameObject canvasObj = new GameObject("AchievementNotificationCanvas");
            canvasObj.transform.SetParent(null);
            DontDestroyOnLoad(canvasObj);

            _notificationCanvas = canvasObj.AddComponent<Canvas>();
            _notificationCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            _notificationCanvas.worldCamera = Camera.main;

            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = canvasResolution;
            scaler.screenMatchMode = canvasMatchWidthOrHeight
                ? CanvasScaler.ScreenMatchMode.MatchWidthOrHeight
                : CanvasScaler.ScreenMatchMode.Expand;

            canvasObj.AddComponent<GraphicRaycaster>();

            GameObject rootObj = new GameObject("NotificationRoot");
            rootObj.transform.SetParent(canvasObj.transform);
            _persistentCanvasRoot = rootObj.transform;

            Debug.Log("[AchievementSystemCore] Created persistent notification canvas.");
        }

        /// <summary>
        /// Get all registered achievement types.
        /// </summary>
        public List<AchievementType> GetAllAchievements()
        {
            return allAchievements;
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