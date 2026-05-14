using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour, IShooting
{
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private string projectileName;
    private PoolManager pool;

    private void Start()
    {
        pool = PoolManager.Instance;    
    }

    public void Shoot()
    {
        pool.SpawnFromPool(projectileName, _projectileSpawnPoint.position, Quaternion.identity);
    }
}
