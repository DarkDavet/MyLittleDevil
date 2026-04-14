using UnityEngine;
using UnityEngine.Events;

namespace CollectibleSystem
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField] private CollectibleType collectibleType;
        [SerializeField] private int quantity = 1;
        [SerializeField] private string uniqueId;
        
        public UnityEvent<Collectible> OnCollected = new UnityEvent<Collectible>();
        
        public CollectibleType Type => collectibleType;
        public int Quantity => quantity;
        public string UniqueId => uniqueId;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Предмет коснулся объекта: " + collision.name);
            var collector = collision.gameObject.GetComponent<ICollectibleCollector>();
            
            if (collector != null)
            {   
                // Check if this specific unique ID has already been collected
                if (!string.IsNullOrEmpty(uniqueId) && CollectibleManager.Instance.IsUniqueIdCollected(uniqueId))
                {
                    Debug.Log("Предмет с уникальным ID уже был собран ранее: " + uniqueId);
                    gameObject.SetActive(false);
                    return;
                }
                
                collector.Collect(this);
                
                Collect();
            }
        }
        
        private void Start()
        {
            // Check if this specific unique ID has already been collected at level start
            if (!string.IsNullOrEmpty(uniqueId) && CollectibleManager.Instance.IsUniqueIdCollected(uniqueId))
            {
                Debug.Log("Предмет с уникальным ID отключен при старте уровня: " + uniqueId);
                gameObject.SetActive(false);
            }
        }
        
        public void Collect()
        {
            OnCollected.Invoke(this);
            Destroy(gameObject);
        }
    }
}
