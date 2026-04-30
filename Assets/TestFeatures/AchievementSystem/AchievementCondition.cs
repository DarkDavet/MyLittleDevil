using UnityEngine;

namespace AchievementSystem
{
    public abstract class AchievementCondition : MonoBehaviour
    {
        [SerializeField] protected string achievementId;
        [SerializeField] protected int progressPerAction = 1;

        public string AchievementId => achievementId;
        public int ProgressPerAction => progressPerAction;

        protected virtual void Awake()
        {
            if (!string.IsNullOrEmpty(achievementId))
            {
                AchievementManager.Instance?.UpdateProgress(achievementId, 0);
            }
        }

        public void NotifyProgress()
        {
            if (!string.IsNullOrEmpty(achievementId))
            {
                AchievementManager.Instance?.UpdateProgress(achievementId, progressPerAction);
            }
        }

        public abstract void Initialize();
    }

    [RequireComponent(typeof(Collider2D))]
    public class CollectibleCondition : AchievementCondition
    {
        [SerializeField] private string collectibleTypeId;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!string.IsNullOrEmpty(collectibleTypeId))
            {
                var collector = collision.GetComponent<CollectibleSystem.ICollectibleCollector>();
                if (collector != null)
                {
                    NotifyProgress();
                }
            }
        }

        public override void Initialize() { }
    }

    public class KillCondition : AchievementCondition
    {
        [SerializeField] private string enemyTag = "Enemy";

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(enemyTag))
            {
                NotifyProgress();
            }
        }

        public override void Initialize() { }
    }

    public class DistanceCondition : AchievementCondition
    {
        [SerializeField] private float distancePerUnit = 1f;
        private float accumulatedDistance = 0f;
        private Player player;

        private void Start()
        {
            player = FindObjectOfType<Player>();
        }

        private void Update()
        {
            if (player == null) return;

            float movement = player.GetMovementDelta();
            accumulatedDistance += movement * distancePerUnit;

            int progress = Mathf.FloorToInt(accumulatedDistance);
            if (progress >= progressPerAction)
            {
                accumulatedDistance -= progress;
                NotifyProgress();
            }
        }

        public override void Initialize() { }
    }

    public class TimeCondition : AchievementCondition
    {
        [SerializeField] private float timePerAction = 60f;
        private float accumulatedTime = 0f;

        private void Update()
        {
            if (Time.timeScale > 0)
            {
                accumulatedTime += Time.deltaTime;

                int progress = Mathf.FloorToInt(accumulatedTime / timePerAction);
                if (progress >= 1)
                {
                    accumulatedTime -= progress * timePerAction;
                    for (int i = 0; i < progress; i++)
                    {
                        NotifyProgress();
                    }
                }
            }
        }

        public override void Initialize() { }
    }

    public class DialogueCondition : AchievementCondition
    {
        private DialogueSystem dialogueSystem;

        private void Start()
        {
            dialogueSystem = GetComponent<DialogueSystem>();
            if (dialogueSystem != null)
            {
                dialogueSystem.OnDialogueFinished.AddListener(OnDialogueFinished);
            }
        }

        private void OnDialogueFinished()
        {
            NotifyProgress();
        }

        public override void Initialize() { }
    }
}