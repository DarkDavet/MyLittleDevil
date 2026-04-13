using UnityEngine;

namespace CollectibleSystem
{
    [CreateAssetMenu(fileName = "NewCollectibleType", menuName = "Collectible System/Collectible Type")]
    public class CollectibleType : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private Sprite icon;
        [SerializeField] private string displayName;
        [SerializeField] private bool isStackable = true;
        [SerializeField] private int maxStackSize = 100;
        [SerializeField] private SaveBehavior saveBehavior = SaveBehavior.OnLevelComplete;
        
        public string Id => id;
        public Sprite Icon => icon;
        public string DisplayName => displayName;
        public bool IsStackable => isStackable;
        public int MaxStackSize => maxStackSize;
        public SaveBehavior SaveBehavior => saveBehavior;
    }
}
