using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private AssetReference assetReference;
    [SerializeField] private AssetLabelReference labelReference;

    private List<GameObject> spawnedObjects = new List<GameObject> { };

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            //Addressables.LoadAssetsAsync<GameObject>(labelReference, (gameobj) => { Debug.Log(gameobj); });
            //assetReference.InstantiateAsync(transform.position , Quaternion.identity);
            /*assetReference.LoadAssetAsync<GameObject>().Completed +=
                (asyncOperationHandle) =>
                {
                    if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        Instantiate(asyncOperationHandle.Result, transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Debug.Log("Failed to load");
                    }
                };*/
            assetReference.InstantiateAsync(transform.position, Quaternion.identity).Completed += (asyncOperation) => { spawnedObjects.Add(asyncOperation.Result); };
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (var item in spawnedObjects)
            {
                assetReference.ReleaseInstance(item);
            }          
        }
    }
}
