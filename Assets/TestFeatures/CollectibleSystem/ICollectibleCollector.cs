using UnityEngine;

namespace CollectibleSystem
{
    public interface ICollectibleCollector
    {
        void Collect(Collectible collectible, int score);
    }
}
