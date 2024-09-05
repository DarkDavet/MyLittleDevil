using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PoolManager;

public class BlueAngelAbility : MonoBehaviour
{
    [SerializeField] private Transform _devil;
    [SerializeField] private BossHealth _angelHealth;
    [SerializeField] private GameObject _abilityPrefab;
    [SerializeField] private Transform[] _spawnPoint;
    [SerializeField] private float _fireRate;
    private PoolManager pool;

    private float nextTimeToFire = 0f;

    private void Start()
    {
        pool = PoolManager.Instance;
    }

    private void Update()
    {
        if (Time.time >= nextTimeToFire && (_devil.position.y > 4.5 || _devil.position.y < - 4.5))
        {
            nextTimeToFire = Time.time + 5f / _fireRate;
            UseAbility();
        }

    }

    private void UseAbility()
    {
        for (int i=0; i < _spawnPoint.Length;i++)
        {
            pool.SpawnFromPool("Bless", _spawnPoint[i].position, Quaternion.identity);
        }
    }

}
