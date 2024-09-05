using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMinion : Minion, IShooting
{
    protected string projectileName = "AllyIce";

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
        pool.SpawnFromPool(projectileName, _projectileSpawnPoint.position, Quaternion.identity);
    }
}
