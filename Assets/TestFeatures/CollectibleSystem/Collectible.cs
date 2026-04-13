using UnityEngine;
using UnityEngine.Events;

namespace CollectibleSystem
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField] private CollectibleType collectibleType;
        [SerializeField] private int quantity = 1;
        [SerializeField] private int scoreMultiplier = 1;
        
        public UnityEvent<Collectible, int> OnCollected = new UnityEvent<Collectible, int>();
        
        public CollectibleType Type => collectibleType;
        public int Quantity => quantity;
        public int ScoreMultiplier => scoreMultiplier;
        
        private void OnTriggerEnter2D(Collider2D collision)
       {
           Debug.Log("Предмет коснулся объекта: " + collision.name);
           var collector = collision.gameObject.GetComponent<ICollectibleCollector>();
    
           if (collector != null)
           {
               int totalScore = quantity * scoreMultiplier;
               collector.Collect(this, totalScore);

               Collect();
           }
        }
        
        public void Collect()
        {
            OnCollected.Invoke(this, quantity * scoreMultiplier);
            Destroy(gameObject);
        }
    }
}
