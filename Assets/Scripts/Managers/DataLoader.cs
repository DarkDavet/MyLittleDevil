using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    [SerializeField]private InventoryObject inventory;
    //private static bool isInitialized = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private void Awake()
    {
        /*if (!isInitialized)
        {
            if (inventory != null)
            {
                inventory.Load();
            }
            else
            {
                Debug.LogError("Inventory is not assigned in the inspector!");
            }
            isInitialized = true;
        }*/
    }
}
