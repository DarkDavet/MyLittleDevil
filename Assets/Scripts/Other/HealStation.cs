using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealStation: MonoBehaviour
{
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float detectionRadius;
    [SerializeField] protected float _fireRate = 1f;
    private PoolManager pool;

    private float nextTimeToShoot = 0f;
    private List<GameObject> damagedObjects = new List<GameObject>();
    private GameObject[] gameObjectInRange;
    private Collider2D[] collidersInRange;

    private void Start()
    {
        pool = PoolManager.Instance;
        collidersInRange = Physics2D.OverlapCircleAll(transform.position, detectionRadius, targetLayer);
        if (collidersInRange.Length > 0)
        {
            gameObjectInRange = ExtractTargetObjects(collidersInRange);
        }
    }

    private void Update()
    {
        if (gameObjectInRange!= null)
        {
            CheckCurrentHealth(gameObjectInRange);
            CheckRecoveredObjects();
        }  
    }

    public void HealShoot()
    {
        Debug.Log("Heal");
        pool.SpawnFromPool("EnemyHeal", _projectileSpawnPoint.position, Quaternion.identity);
    }

    public void CheckRecoveredObjects()
    {
        if (damagedObjects != null)
        {
            damagedObjects.RemoveAll(obj => obj.GetComponent<EnemyHealth>().currentHealth == obj.GetComponent<EnemyHealth>().maxHealth);
        }      
    }

    public void CheckCurrentHealth(GameObject[] gameObjectsInRange)
    {
        for (int i = 0; i < gameObjectsInRange.Length; i++)
        {
            if (gameObjectsInRange[i] != null)
            {
                var enemyHealth = gameObjectsInRange[i].GetComponent<EnemyHealth>();
                if (enemyHealth != null && enemyHealth.currentHealth < enemyHealth.maxHealth && enemyHealth.currentHealth > 0)
                {
                    if (Time.time >= nextTimeToShoot)
                    {
                        nextTimeToShoot = Time.time + 1f / _fireRate;
                        HealShoot();
                    }
                    //if (!damagedObjects.Contains(gameObjectsInRange[i]))
                    //{
                    //    Debug.Log("Check");
                    //    damagedObjects.Add(gameObjectsInRange[i]);
                    //}
                }
            }
        }
    }

    public GameObject[] ExtractTargetObjects(Collider2D[] collidersInRange)
    {
        GameObject[] gameObjectsInRange = new GameObject[collidersInRange.Length];
        for (int i = 0; i < collidersInRange.Length; i++)
        {
            gameObjectsInRange[i] = collidersInRange[i].gameObject;
        }
        return gameObjectsInRange;
    }
}
