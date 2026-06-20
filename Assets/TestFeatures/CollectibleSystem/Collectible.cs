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
            var collector = collision.gameObject.GetComponent<ICollectibleCollector>();

            if (collector != null)
            {
  
                collector.Collect(this);
                Collect();
            }
        }
        
        private void Start()
        {
            if (!string.IsNullOrEmpty(uniqueId) && CollectibleManager.Instance.IsUniqueIdCollected(uniqueId))
            {
                Debug.Log($"[Collectible] Предмет {uniqueId} отключен при старте, так как уже был собран ранее.");
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
