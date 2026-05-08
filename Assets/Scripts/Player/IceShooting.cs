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
        float currentTime = TimeManager.Instance.PlayerActiveTime;

        if (currentTime >= nextTimeToShoot)
        {
            float cooldownMultiplier = 1f;

            // Если замедлены ВСЕ (включая игрока), увеличиваем КД
            if (TimeManager.Instance.IsSlowedDown && !TimeManager.Instance.IgnoreTimeScale)
            {
                cooldownMultiplier = 1f / Time.timeScale;
            }

            // Рассчитываем время следующего выстрела
            nextTimeToShoot = currentTime + (1f / _fireRate) * cooldownMultiplier;

            animator.SetTrigger("IceShoot");
            PoolManager.Instance.SpawnFromPool("Ice", _projectileSpawnPoint.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("IceAttack");
        }
    }
}
