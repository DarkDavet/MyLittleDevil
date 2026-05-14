using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour, IShooting
{
    [System.Serializable]
    public struct AlternativeProjectile
    {
        public string projectileName; 
        [Range(0, 100)] public float spawnChance; 
    }

    [SerializeField] private Transform _projectileSpawnPoint;
    [Header("Основной снаряд (по умолчанию)")]
    [SerializeField] private string defaultProjectileName;

    [Header("Редкие/Альтернативные снаряды")]
    [SerializeField] private AlternativeProjectile[] alternativeProjectiles;

    private PoolManager pool;

    private void Start()
    {
        pool = PoolManager.Instance;
    }

    public void Shoot()
    {
        string finalProjectile = SelectProjectile();
        pool.SpawnFromPool(finalProjectile, _projectileSpawnPoint.position, Quaternion.identity);
    }

    private string SelectProjectile()
    {
        if (alternativeProjectiles == null || alternativeProjectiles.Length == 0)
        {
            return defaultProjectileName;
        }

        float roll = Random.Range(0f, 100f);
        float currentWeight = 0f;

        foreach (var alt in alternativeProjectiles)
        {
            currentWeight += alt.spawnChance;
            if (roll <= currentWeight)
            {
                return alt.projectileName; 
            }
        }

        return defaultProjectileName;
    }
}
