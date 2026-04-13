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
            
            // OnCollection items go directly to collected items and save immediately
            if (behavior == SaveBehavior.OnCollection)
            {
                if (collectedItems.ContainsKey(collectible.Type.Id))
                {
                    collectedItems[collectible.Type.Id] += collectible.Quantity;
                }
                else
                {
                    collectedItems[collectible.Type.Id] = collectible.Quantity;
                }
                SaveToPlayerPrefs();
            }
            // OnLevelComplete items go to temporary storage
            else if (behavior == SaveBehavior.OnLevelComplete)
            {
                if (temporaryItems.ContainsKey(collectible.Type.Id))
                {
                    temporaryItems[collectible.Type.Id] += collectible.Quantity;
                }
                else
                {
                    temporaryItems[collectible.Type.Id] = collectible.Quantity;
                }
            }
            
            Debug.Log("Collected: " + collectible.Type.DisplayName + " x" + collectible.Quantity + " (Save: " + behavior + ")");
            OnCollectibleCollected?.Invoke(collectible);
        }
        
        public void SaveTemporaryItems()
        {
            Debug.Log("Saving temporary items to collected items. Count: " + temporaryItems.Count);
            
            // Move all temporary items to collected items
            foreach (var item in temporaryItems)
            {
                if (item.Value > 0) // Only save if count > 0
                {
                    if (collectedItems.ContainsKey(item.Key))
                    {
                        collectedItems[item.Key] += item.Value;
                        Debug.Log("Added to existing: " + item.Key + " = " + collectedItems[item.Key]);
                    }
                    else
                    {
                        collectedItems[item.Key] = item.Value;
                        Debug.Log("New item: " + item.Key + " = " + item.Value);
                    }
                }
            }
            Debug.Log("Total collected items: " + collectedItems.Count);
            
            SaveToPlayerPrefs();
            temporaryItems.Clear();
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
            PlayerPrefs.Save();
            Debug.Log("Save complete. Total items saved: " + index);
        }
        
        public void LoadFromPlayerPrefs()
        {
            int count = PlayerPrefs.GetInt("ItemCount_Total", 0);
            Debug.Log("Loading from PlayerPrefs. Found " + count + " items to load");
            
            collectedItems.Clear();
            
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
            Debug.Log("Load complete. Total items loaded: " + collectedItems.Count);
        }
        
        public int GetItemCount(string itemId)
        {
            if (collectedItems.TryGetValue(itemId, out int count))
            {
                return count;
            }
            return 0;
        }
        
        public Dictionary<string, int> GetAllCollectedItems()
        {
            return collectedItems;
        }
    }
}
