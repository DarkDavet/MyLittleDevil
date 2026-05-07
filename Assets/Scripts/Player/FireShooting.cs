using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles fire projectile shooting for the player.
/// </summary>
public class FireShooting : Shooting
{
    private float _nextShootTime;
    /// <summary>
    /// Shoots a fire projectile from the player's position.
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
            pool.SpawnFromPool("Fire", _projectileSpawnPoint.position, Quaternion.identity);

            FindObjectOfType<AudioManager>().Play("FireAttack");
        }
    }
}
