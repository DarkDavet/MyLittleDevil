using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles ice projectile shooting for the player.
/// </summary>
public class IceShooting : Shooting
{
    public override void Shoot()
    {
        float currentUnscaledTime = Time.unscaledTime;

        if (currentUnscaledTime >= nextTimeToShoot)
        {
            float cooldownMultiplier = 1f;

            if (TimeManager.Instance.IsSlowedDown && !TimeManager.Instance.IgnoreTimeScale)
            {
                cooldownMultiplier = 1f / Time.timeScale;
            }

            // Устанавливаем время следующего выстрела на основе unscaledTime
            nextTimeToShoot = currentUnscaledTime + (1f / _fireRate) * cooldownMultiplier;

            animator.SetTrigger("IceShoot");
            PoolManager.Instance.SpawnFromPool("Ice", _projectileSpawnPoint.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("IceAttack");
        }
    }
}
