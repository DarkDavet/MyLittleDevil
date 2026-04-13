using System.Collections.Generic;
using UnityEngine;

namespace CollectibleSystem
{
    public class CollectibleManager : MonoBehaviour
    {
        public static CollectibleManager Instance { get; private set; }
        
        public delegate void CollectibleCollectedHandler(Collectible collectible, int score);
        public event CollectibleCollectedHandler OnCollectibleCollected;
        
        private Dictionary<string, int> collectedItems = new Dictionary<string, int>();
        private int totalScore;
        
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
        }
        
        public void Collect(Collectible collectible, int score)
        {
            if (collectible.Type == null) return;
            
            totalScore += score;
            
            if (collectedItems.ContainsKey(collectible.Type.Id))
            {
                collectedItems[collectible.Type.Id] += collectible.Quantity;
            }
            else
            {
                collectedItems[collectible.Type.Id] = collectible.Quantity;
            }
            
            Debug.Log("Collected: " + collectible.Type.DisplayName + " x" + collectible.Quantity + " (Score: " + score + ")");
            Debug.Log("Total: " + collectible.Type.DisplayName + " x" + ". count = " + GetItemCount("coin"));
            OnCollectibleCollected?.Invoke(collectible, score);
        }
        
        public int GetItemCount(string itemId)
        {
            if (collectedItems.TryGetValue(itemId, out int count))
            {
                return count;
            }
            return 0;
        }
        
        public int GetTotalScore()
        {
            return totalScore;
        }
        
        public Dictionary<string, int> GetAllCollectedItems()
        {
            return collectedItems;
        }
    }
}
