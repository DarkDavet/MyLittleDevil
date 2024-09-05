using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour, IShooting
{
    [SerializeField] private Transform _arrowSpawnPoint;
    private PoolManager pool;

    private void Start()
    {
        pool = PoolManager.Instance;    
    }

    public void Shoot()
    {
        pool.SpawnFromPool("Arrow", _arrowSpawnPoint.position, Quaternion.identity);
    }
}
