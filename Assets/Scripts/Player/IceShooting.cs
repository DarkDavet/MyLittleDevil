using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShooting : Shooting
{
    public override void Shoot()
    {
        if (_player.CheckBirdStatus() && Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.time + 1f / _fireRate;
            animator.SetTrigger("IceShoot");
            pool.SpawnFromPool("Ice", _projectileSpawnPoint.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("IceAttack");
        }
    }
}
