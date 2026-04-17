using System.Collections.Generic;
using UnityEngine;

namespace CollectibleSystem
{
    public enum SaveBehavior
    {
        OnCollection,  // Save immediately when collected
        OnLevelComplete,  // Save only when level is completed
        OnGameEnd,  // Save when game ends
        Never  // Never save this type
    }

    public class CollectibleManager : MonoBehaviour
    {
        [System.Serializable]
        public class GlobalSaveData
        {
            public List<string> keys;
            public List<int> values;
            public List<string> uniqueIds;
        }
        public static CollectibleManager Instance { get; private set; }

        public delegate void CollectibleCollectedHandler(Collectible collectible);
        public event CollectibleCollectedHandler OnCollectibleCollected;

        private Dictionary<string, int> collectedItems = new Dictionary<string, int>();
        private Dictionary<string, int> temporaryItems = new Dictionary<string, int>();

        private HashSet<string> collectedUniqueIds = new HashSet<string>();
        private HashSet<string> tmp_collectedUniqueIds = new HashSet<string>();


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            LoadFromPlayerPrefs();
        }

        public void Collect(Collectible collectible)
        {
            if (collectible.Type == null) return;

            SaveBehavior behavior = collectible.Type.SaveBehavior;
            string uid = collectible.UniqueId;

            // 1. Сначала ВСЕГДА обновляем статистику уровня для UI (только 1 раз!)
            if (behavior != SaveBehavior.Never)
            {
                LevelStatsManager.Instance?.UpdateStats(collectible.Type.Id, collectible.Quantity);
            }

            // 2. Затем определяем логику сохранения
            if (behavior == SaveBehavior.OnCollection)
            {
                UpdateItemCount(collectedItems, collectible.Type.Id, collectible.Quantity);
                if (!string.IsNullOrEmpty(uid)) collectedUniqueIds.Add(uid);

                SaveToPlayerPrefs();
                // Сохраняем статистику уровня немедленно для этого типа
                LevelStatsManager.Instance?.SaveCurrentLevelStats();
            }
            else if (behavior != SaveBehavior.OnCollection && behavior != SaveBehavior.Never)
            {
                UpdateItemCount(temporaryItems, collectible.Type.Id, collectible.Quantity);
                if (!string.IsNullOrEmpty(uid)) tmp_collectedUniqueIds.Add(uid);
                // Здесь SaveCurrentLevelStats НЕ вызываем, ждем конца уровня
            }

            OnCollectibleCollected?.Invoke(collectible);
        }

        // Вспомогательный метод, чтобы не дублировать код сложения
        private void UpdateItemCount(Dictionary<string, int> dict, string id, int amount)
        {
            if (dict.ContainsKey(id)) dict[id] += amount;
            else dict[id] = amount;
        }

        public void SaveTemporaryItems()
        {
            Debug.Log("Saving temporary items (OnLevelComplete items)");

            // Переносим количества
            foreach (var item in temporaryItems)
            {
                UpdateItemCount(collectedItems, item.Key, item.Value);
                Debug.Log("  Saved temporary: " + item.Key + " = " + item.Value);
            }

            // Переносим уникальные ID
            collectedUniqueIds.UnionWith(tmp_collectedUniqueIds);

            // ВАЖНО: Очищаем оба временных списка
            temporaryItems.Clear();
            tmp_collectedUniqueIds.Clear();

            SaveToPlayerPrefs();

            // Save level stats to persist the updated counts
            LevelStatsManager.Instance?.SaveCurrentLevelStats();

            Debug.Log("Temporary items saved successfully");
        }

        public void SaveToPlayerPrefs()
        {
            var data = new GlobalSaveData
            {
                keys = new List<string>(collectedItems.Keys),
                values = new List<int>(collectedItems.Values),
                uniqueIds = new List<string>(collectedUniqueIds)
            };
            PlayerPrefs.SetString("GlobalCollectibles", JsonUtility.ToJson(data));
        }

        public void LoadFromPlayerPrefs()
        {
            collectedItems.Clear();
            collectedUniqueIds.Clear();

            if (PlayerPrefs.HasKey("GlobalCollectibles"))
            {
                string json = PlayerPrefs.GetString("GlobalCollectibles");
                GlobalSaveData data = JsonUtility.FromJson<GlobalSaveData>(json);

                if (data != null)
                {
                    // Восстанавливаем словарь предметов
                    for (int i = 0; i < data.keys.Count; i++)
                    {
                        collectedItems[data.keys[i]] = data.values[i];
                    }

                    // Восстанавливаем уникальные ID
                    if (data.uniqueIds != null)
                    {
                        collectedUniqueIds = new HashSet<string>(data.uniqueIds);
                    }

                    Debug.Log($"Загружено: предметов {collectedItems.Count}, уникальных ID {collectedUniqueIds.Count}");
                }
            }
        }

        public int GetItemCount(string itemId)
        {
            if (collectedItems.TryGetValue(itemId, out int count))
            {
                return count;
            }
            return 0;
        }

        public int GetTemporaryItemCount(string itemId)
        {
            if (temporaryItems.TryGetValue(itemId, out int count))
            {
                return count;
            }
            return 0;
        }

        public bool IsUniqueIdCollected(string uniqueId)
        {
            return collectedUniqueIds.Contains(uniqueId);
        }

        public Dictionary<string, int> GetAllCollectedItems()
        {
            return collectedItems;
        }
    }
}
