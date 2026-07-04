using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMinion : Minion, IShooting
{
    protected string projectileName = "AllyFire";

    public override void Attack()
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
        PoolManager.Instance.SpawnFromPool(projectileName, _projectileSpawnPoint.position, Quaternion.identity);
    }
}
