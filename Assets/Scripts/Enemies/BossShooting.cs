using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooting: MonoBehaviour, IShooting
{
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _arrowSpawnPoint;
    [SerializeField] private float _maxfireRate = 3f;
    [SerializeField] private float _minfireRate = 1f;
    private PoolManager pool;

    private float _fireRate;
    private float nextTimeToFire = 0f;

    private void Start()
    {
        pool = PoolManager.Instance;
    }

    private void Update()
    {
        if (Time.time >= nextTimeToFire)
        {
            _fireRate = Random.Range(_minfireRate, _maxfireRate);
            nextTimeToFire = Time.time + 1f / _fireRate;
            Shoot();
        }
    }

    public void Shoot()
    {
        pool.SpawnFromPool("Lightning", _arrowSpawnPoint.position, Quaternion.identity);
    }
}
