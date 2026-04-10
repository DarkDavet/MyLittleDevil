using UnityEngine;

namespace CollectibleSystem
{
    public class CollectibleEventData
    {
        public Collectible Collectible { get; }
        public int Score { get; }
        
        public CollectibleEventData(Collectible collectible, int score)
        {
            Collectible = collectible;
            Score = score;
        }
    }
}
