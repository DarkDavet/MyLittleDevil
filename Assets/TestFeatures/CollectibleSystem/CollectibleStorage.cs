using System.Collections.Generic;
using UnityEngine;

namespace CollectibleSystem
{
    public class CollectibleStorage : MonoBehaviour
    {
        [SerializeField] private List<CollectibleType> collectibleTypes;
        
        public static CollectibleStorage Instance { get; private set; }
        
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
        
        public CollectibleType GetCollectibleType(string id)
        {
            foreach (var type in collectibleTypes)
            {
                if (type.Id == id)
                {
                    return type;
                }
            }
            return null;
        }
        
        public List<CollectibleType> GetAllCollectibleTypes()
        {
            return collectibleTypes;
        }
    }
}
