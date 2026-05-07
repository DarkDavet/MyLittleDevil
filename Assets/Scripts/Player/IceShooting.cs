using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles ice projectile shooting for the player.
/// </summary>
public class IceShooting : Shooting
{
    private float _nextShootTime;
    /// <summary>
    /// Shoots an ice projectile from the player's position.
    /// </summary>
    public override void Shoot()
    {
        float currentTime = TimeManager.Instance.IgnoreTimeScale ? Time.unscaledTime : Time.time;

        if (currentTime >= _nextShootTime)
        {
            // Рассчитываем время следующего выстрела
            _nextShootTime = currentTime + 1f / _fireRate;

            animator.SetTrigger("IceShoot");

            // Спавним снаряд (логика скорости уже внутри самого снаряда через multiplier)
            pool.SpawnFromPool("Ice", _projectileSpawnPoint.position, Quaternion.identity);

            FindObjectOfType<AudioManager>().Play("IceAttack");
        }
    }
}
