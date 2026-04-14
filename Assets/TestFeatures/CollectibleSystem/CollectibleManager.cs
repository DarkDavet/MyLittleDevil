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
            string uid = collectible.UniqueId; // Сохраняем локально для безопасности

            if (behavior == SaveBehavior.OnCollection)
            {
                UpdateItemCount(collectedItems, collectible.Type.Id, collectible.Quantity);
                if (!string.IsNullOrEmpty(uid)) collectedUniqueIds.Add(uid); // Сразу в постоянный
                SaveToPlayerPrefs();
            }
            else if (behavior == SaveBehavior.OnLevelComplete)
            {
                UpdateItemCount(temporaryItems, collectible.Type.Id, collectible.Quantity);
                if (!string.IsNullOrEmpty(uid)) tmp_collectedUniqueIds.Add(uid); // Во временный
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
            // Переносим количества
            foreach (var item in temporaryItems)
            {
                UpdateItemCount(collectedItems, item.Key, item.Value);
            }

            // Переносим уникальные ID
            collectedUniqueIds.UnionWith(tmp_collectedUniqueIds);

            // ВАЖНО: Очищаем оба временных списка
            temporaryItems.Clear();
            tmp_collectedUniqueIds.Clear();

            SaveToPlayerPrefs();
        }

        public void SaveToPlayerPrefs()
        {
            Debug.Log("Saving " + collectedItems.Count + " items to PlayerPrefs");
            int index = 0;
            foreach (var item in collectedItems)
            {
                if (item.Value > 0) // Only save if count > 0
                {
                    PlayerPrefs.SetString("ItemId_" + index, item.Key);
                    PlayerPrefs.SetInt("ItemCount_" + index, item.Value);
                    Debug.Log("  Saved: " + item.Key + " = " + item.Value);
                    index++;
                }
            }
            PlayerPrefs.SetInt("ItemCount_Total", index);
            
            // Save unique IDs
            index = 0;
            foreach (var uniqueId in collectedUniqueIds)
            {
                PlayerPrefs.SetString("UniqueId_" + index, uniqueId);
                index++;
            }
            PlayerPrefs.SetInt("UniqueIdCount_Total", index);
            
            PlayerPrefs.Save();
            Debug.Log("Save complete. Total items saved: " + collectedItems.Count + ", Unique IDs saved: " + collectedUniqueIds.Count);
        }
        
        public void LoadFromPlayerPrefs()
        {
            int count = PlayerPrefs.GetInt("ItemCount_Total", 0);
            Debug.Log("Loading from PlayerPrefs. Found " + count + " items to load");
            
            collectedItems.Clear();
            collectedUniqueIds.Clear();
            
            for (int i = 0; i < count; i++)
            {
                string id = PlayerPrefs.GetString("ItemId_" + i, string.Empty);
                int countValue = PlayerPrefs.GetInt("ItemCount_" + i, 0);
                
                if (!string.IsNullOrEmpty(id) && countValue > 0)
                {
                    collectedItems[id] = countValue;
                    Debug.Log("  Loaded: " + id + " = " + countValue);
                }
            }
            
            // Load unique IDs
            int uniqueIdCount = PlayerPrefs.GetInt("UniqueIdCount_Total", 0);
            for (int i = 0; i < uniqueIdCount; i++)
            {
                string uniqueId = PlayerPrefs.GetString("UniqueId_" + i, string.Empty);
                if (!string.IsNullOrEmpty(uniqueId))
                {
                    collectedUniqueIds.Add(uniqueId);
                    Debug.Log("  Loaded Unique ID: " + uniqueId);
                }
            }
            
            Debug.Log("Load complete. Total items loaded: " + collectedItems.Count + ", Unique IDs loaded: " + collectedUniqueIds.Count);
        }
        
        public int GetItemCount(string itemId)
        {
            if (collectedItems.TryGetValue(itemId, out int count))
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
