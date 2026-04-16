using UnityEngine;

public class CollectibleCounter : MonoBehaviour
{
    [SerializeField] private CollectibleSystem.CollectibleType collectibleType;
    [SerializeField] private SceneData sceneData; // Assign this in the inspector
    
    private int count = 0;
    
    private void Start()
    {
        if (collectibleType != null)
        {
            // Find all collectibles of this type in the scene
            var collectibles = FindObjectsOfType<CollectibleSystem.Collectible>();
            
            foreach (var collectible in collectibles)
            {
                if (collectible.Type == collectibleType)
                {
                    count++;
                }
            }
            
            Debug.Log("Found " + count + " collectibles of type: " + collectibleType.DisplayName);
        }
    }
}