using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShooting : Shooting
{
    public override void Shoot()
    {
        if (Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.time + 1f / _fireRate;
            animator.SetTrigger("Shoot");
            pool.SpawnFromPool("Fire", _projectileSpawnPoint.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("FireAttack");
        }
    }
}
