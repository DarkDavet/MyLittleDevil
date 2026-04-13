using UnityEngine;
using UnityEngine.Events;

namespace CollectibleSystem
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField] private CollectibleType collectibleType;
        [SerializeField] private int quantity = 1;
        
        public UnityEvent<Collectible, int> OnCollected = new UnityEvent<Collectible, int>();
        
        public CollectibleType Type => collectibleType;
        public int Quantity => quantity;
        
        private void OnTriggerEnter2D(Collider2D collision)
       {
           Debug.Log("Предмет коснулся объекта: " + collision.name);
           var collector = collision.gameObject.GetComponent<ICollectibleCollector>();
    
           if (collector != null)
           {
               collector.Collect(this);

               Collect();
           }
        }
        
        public void Collect()
        {
            OnCollected.Invoke(this, quantity);
            Destroy(gameObject);
        }
    }
}
