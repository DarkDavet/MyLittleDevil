using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HealStation: MonoBehaviour
{
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] protected float _fireRate = 1f;

    private float nextTimeToShoot = 0f;
    // Кэшируем массив, чтобы не пересоздавать его каждый раз
    private Collider2D[] results = new Collider2D[10];

    private void Update()
    {
        if (Time.time >= nextTimeToShoot)
        {
            SearchAndHeal();
        }
    }

    private void SearchAndHeal()
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, detectionRadius, results, targetLayer);

        for (int i = 0; i < count; i++)
        {
            // Используем TryGetComponent для оптимизации
            if (results[i].TryGetComponent<EnemyHealth>(out var health))
            {
                // Проверяем: ранен ли и жив ли
                if (health.currentHealth < health.maxHealth && health.currentHealth > 0)
                {
                    // Передаем трансформ найденной цели в метод выстрела
                    HealShoot(health.transform);

                    // Устанавливаем кулдаун
                    nextTimeToShoot = Time.time + 1f / _fireRate;
                    break;
                }
            }
        }
    }

    // Добавили параметр Transform target
    public void HealShoot(Transform target)
    {
        var projectileGo = PoolManager.Instance.SpawnFromPool("EnemyHeal", _projectileSpawnPoint.position, Quaternion.identity);

        // Передаем цель в скрипт снаряда
        if (projectileGo.TryGetComponent<HealProjectile>(out var projectileScript))
        {
            projectileScript.SetTarget(target);
        }
    }

    // Визуализация радиуса в редакторе для удобства настройки
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
