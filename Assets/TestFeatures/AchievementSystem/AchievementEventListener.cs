using UnityEngine;

namespace AchievementSystem
{
    /// <summary>
    /// Attach this component to any GameObject in the scene to automatically
    /// update achievement progress when specific events occur.
    /// 
    /// Use the AchievementEvents static class from code to trigger events.
    /// Or use the UnityEvents in the Inspector for visual setup.
    /// </summary>
    public class AchievementEventListener : MonoBehaviour
    {
        [System.Serializable]
        public class AchievementTrigger
        {
            [SerializeField] private string achievementId;
            [SerializeField] private int progressAmount = 1;

            public string AchievementId => achievementId;
            public int ProgressAmount => progressAmount;
        }

        [Header("Event Triggers")]
        [Tooltip("Achievements to update when an enemy is killed")]
        [SerializeField] private AchievementTrigger[] onKillTriggers;

        [Tooltip("Achievements to update when an item is collected")]
        [SerializeField] private AchievementTrigger[] onCollectTriggers;

        [Tooltip("Achievements to update when damage is dealt")]
        [SerializeField] private AchievementTrigger[] onDamageDealtTriggers;

        [Tooltip("Achievements to update when player heals")]
        [SerializeField] private AchievementTrigger[] onHealedTriggers;

        [Tooltip("Achievements to update when distance is traveled")]
        [SerializeField] private AchievementTrigger[] onDistanceTraveledTriggers;

        [Tooltip("Achievements to update when a dialogue finishes")]
        [SerializeField] private AchievementTrigger[] onDialogueFinishedTriggers;

        [Tooltip("Achievements to update when a level is completed")]
        [SerializeField] private AchievementTrigger[] onLevelCompleteTriggers;

        [Tooltip("Achievements to update when a chest is opened")]
        [SerializeField] private AchievementTrigger[] onChestOpenedTriggers;

        private void OnEnable()
        {
            AchievementEvents.OnKill += OnKill;
            AchievementEvents.OnCollect += OnCollect;
            AchievementEvents.OnDamageDealt += OnDamageDealt;
            AchievementEvents.OnHealed += OnHealed;
            AchievementEvents.OnDistanceTraveled += OnDistanceTraveled;
            AchievementEvents.OnDialogueFinished += OnDialogueFinished;
            AchievementEvents.OnLevelComplete += OnLevelComplete;
            AchievementEvents.OnChestOpened += OnChestOpened;
        }

        private void OnDisable()
        {
            AchievementEvents.OnKill -= OnKill;
            AchievementEvents.OnCollect -= OnCollect;
            AchievementEvents.OnDamageDealt -= OnDamageDealt;
            AchievementEvents.OnHealed -= OnHealed;
            AchievementEvents.OnDistanceTraveled -= OnDistanceTraveled;
            AchievementEvents.OnDialogueFinished -= OnDialogueFinished;
            AchievementEvents.OnLevelComplete -= OnLevelComplete;
            AchievementEvents.OnChestOpened -= OnChestOpened;
        }

        private void OnKill()
        {
            if (onKillTriggers != null)
            {
                foreach (var trigger in onKillTriggers)
                {
                    if (!string.IsNullOrEmpty(trigger.AchievementId))
                    {
                        AchievementManager.Instance?.UpdateProgress(trigger.AchievementId, trigger.ProgressAmount);
                    }
                }
            }
        }

        private void OnCollect(CollectibleSystem.Collectible collectible)
        {
            if (onCollectTriggers != null)
            {
                foreach (var trigger in onCollectTriggers)
                {
                    if (!string.IsNullOrEmpty(trigger.AchievementId))
                    {
                        AchievementManager.Instance?.UpdateProgress(trigger.AchievementId, trigger.ProgressAmount);
                    }
                }
            }
        }

        private void OnDamageDealt(float damage)
        {
            if (onDamageDealtTriggers != null)
            {
                foreach (var trigger in onDamageDealtTriggers)
                {
                    if (!string.IsNullOrEmpty(trigger.AchievementId))
                    {
                        AchievementManager.Instance?.UpdateProgress(trigger.AchievementId, trigger.ProgressAmount);
                    }
                }
            }
        }

        private void OnHealed(int amount)
        {
            if (onHealedTriggers != null)
            {
                foreach (var trigger in onHealedTriggers)
                {
                    if (!string.IsNullOrEmpty(trigger.AchievementId))
                    {
                        AchievementManager.Instance?.UpdateProgress(trigger.AchievementId, trigger.ProgressAmount);
                    }
                }
            }
        }

        private void OnDistanceTraveled(float distance)
        {
            if (onDistanceTraveledTriggers != null)
            {
                foreach (var trigger in onDistanceTraveledTriggers)
                {
                    if (!string.IsNullOrEmpty(trigger.AchievementId))
                    {
                        AchievementManager.Instance?.UpdateProgress(trigger.AchievementId, trigger.ProgressAmount);
                    }
                }
            }
        }

        private void OnDialogueFinished(string dialogueId)
        {
            if (onDialogueFinishedTriggers != null)
            {
                foreach (var trigger in onDialogueFinishedTriggers)
                {
                    if (!string.IsNullOrEmpty(trigger.AchievementId))
                    {
                        AchievementManager.Instance?.UpdateProgress(trigger.AchievementId, trigger.ProgressAmount);
                    }
                }
            }
        }

        private void OnLevelComplete(string levelId)
        {
            if (onLevelCompleteTriggers != null)
            {
                foreach (var trigger in onLevelCompleteTriggers)
                {
                    if (!string.IsNullOrEmpty(trigger.AchievementId))
                    {
                        AchievementManager.Instance?.UpdateProgress(trigger.AchievementId, trigger.ProgressAmount);
                    }
                }
            }
        }

        private void OnChestOpened(string chestId)
        {
            if (onChestOpenedTriggers != null)
            {
                foreach (var trigger in onChestOpenedTriggers)
                {
                    if (!string.IsNullOrEmpty(trigger.AchievementId))
                    {
                        AchievementManager.Instance?.UpdateProgress(trigger.AchievementId, trigger.ProgressAmount);
                    }
                }
            }
        }
    }
}