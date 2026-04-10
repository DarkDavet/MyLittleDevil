using UnityEngine;

namespace CollectibleSystem
{
    [CreateAssetMenu(fileName = "NewCollectibleType", menuName = "Collectible System/Collectible Type")]
    public class CollectibleType : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private Sprite icon;
        [SerializeField] private string displayName;
        [SerializeField] private int baseScoreValue = 10;
        [SerializeField] private bool isStackable = true;
        [SerializeField] private int maxStackSize = 100;
        
        public string Id => id;
        public Sprite Icon => icon;
        public string DisplayName => displayName;
        public int BaseScoreValue => baseScoreValue;
        public bool IsStackable => isStackable;
        public int MaxStackSize => maxStackSize;
    }
}
