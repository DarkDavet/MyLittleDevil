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
    
    [System.Serializable]
    public class CollectibleSaveSettings
    {
        public CollectibleType type;
        public SaveBehavior saveBehavior;
    }
    
    public class CollectibleManager : MonoBehaviour
    {
        public static CollectibleManager Instance { get; private set; }
        
        public delegate void CollectibleCollectedHandler(Collectible collectible);
        public event CollectibleCollectedHandler OnCollectibleCollected;
        
        [SerializeField] private List<CollectibleSaveSettings> saveSettings = new List<CollectibleSaveSettings>();
        
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
            
            // Check save behavior for this type
            SaveBehavior behavior = GetSaveBehavior(collectible.Type.Id);
            
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
            
            Debug.Log("Collected: " + collectible.Type.DisplayName + " x" + collectible.Quantity);
            OnCollectibleCollected?.Invoke(collectible);
        }
        
        public void SaveTemporaryItems()
        {
            // Move all temporary items to collected items
            foreach (var item in temporaryItems)
            {
                if (GetSaveBehavior(item.Key) != SaveBehavior.Never)
                {
                    if (collectedItems.ContainsKey(item.Key))
                    {
                        collectedItems[item.Key] += item.Value;
                    }
                    else
                    {
                        collectedItems[item.Key] = item.Value;
                    }
                }
            }
            SaveToPlayerPrefs();
            temporaryItems.Clear();
        }
        
        public void SaveToPlayerPrefs()
        {
            int index = 0;
            foreach (var item in collectedItems)
            {
                if (GetSaveBehavior(item.Key) != SaveBehavior.Never)
                {
                    PlayerPrefs.SetString("ItemId_" + index, item.Key);
                    PlayerPrefs.SetInt("ItemCount_" + index, item.Value);
                    index++;
                }
            }
            PlayerPrefs.SetInt("ItemCount_Total", index);
            PlayerPrefs.Save();
        }
        
        public void LoadFromPlayerPrefs()
        {
            int count = PlayerPrefs.GetInt("ItemCount_Total", 0);
            for (int i = 0; i < count; i++)
            {
                string id = PlayerPrefs.GetString("ItemId_" + i, string.Empty);
                int countValue = PlayerPrefs.GetInt("ItemCount_" + i, 0);
                
                if (!string.IsNullOrEmpty(id))
                {
                    collectedItems[id] = countValue;
                }
            }
        }
        
        private SaveBehavior GetSaveBehavior(string itemId)
        {
            foreach (var setting in saveSettings)
            {
                if (setting.type != null && setting.type.Id == itemId)
                {
                    return setting.saveBehavior;
                }
            }
            return SaveBehavior.OnGameEnd; // Default behavior
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
