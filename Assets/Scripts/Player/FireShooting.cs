using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles fire projectile shooting for the player.
/// </summary>
public class FireShooting : Shooting
{
    /// <summary>
    /// Shoots a fire projectile from the player's position.
    /// </summary>
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
