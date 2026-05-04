using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HealStation: MonoBehaviour
{
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private LayerMask targetLayer; // Выбираешь Player или Enemy в инспекторе
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] protected float _fireRate = 1f;
    [SerializeField] private int healAmount = 1; // Сколько лечит снаряд
    [SerializeField] private float _spread = 10f;

    private float nextTimeToShoot = 0f;
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
            // Ищем любой компонент, реализующий IHealable
            var health = results[i].GetComponent<IHealable>();

            if (health != null && health.CurrentHealth < health.MaxHealth && health.CurrentHealth > 0)
            {
                HealShoot(results[i].transform);
                nextTimeToShoot = Time.time + 1f / _fireRate;
                break;
            }
        }
    }

    public void HealShoot(Transform target)
    {
        var projectileGo = PoolManager.Instance.SpawnFromPool("EnemyHeal", _projectileSpawnPoint.position, Quaternion.identity);
        if (projectileGo.TryGetComponent<HealProjectile>(out var proj))
        {
            proj.SetTarget(target);

            float randomOffset = Random.Range(-_spread, _spread);
            projectileGo.transform.Rotate(0, 0, randomOffset);

            proj.SetHealAmount(healAmount); // Передаем силу лечения снаряду
        }
    }
}
