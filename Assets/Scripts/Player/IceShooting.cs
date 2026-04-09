using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles ice projectile shooting for the player.
/// </summary>
public class IceShooting : Shooting
{
    /// <summary>
    /// Shoots an ice projectile from the player's position.
    /// </summary>
    public override void Shoot()
    {
        if (Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.time + 1f / _fireRate;
            animator.SetTrigger("IceShoot");
            pool.SpawnFromPool("Ice", _projectileSpawnPoint.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("IceAttack");
        }
    }
}
