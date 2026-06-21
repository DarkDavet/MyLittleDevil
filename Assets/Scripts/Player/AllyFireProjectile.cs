using System.Collections;
using UnityEngine;

/// <summary>
/// Represents a fire projectile fired by ally units.
/// </summary>
public class AllyFireProjectile : BaseProjectile
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 10f; 
    [SerializeField] private LayerMask enemyLayer;    

    private Transform enemy;
    private readonly Collider2D[] detectionBuffer = new Collider2D[10];

    public override void OnObjectSpawn()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        enemy = FindClosestEnemyInRadius();

        if (rb != null)
        {
            if (timer != null) StopCoroutine(timer);
            timer = StartCoroutine(ReturnToPoolAfterTime());

            if (enemy != null)
            {
                Vector2 direction = (enemy.position - transform.position).normalized;
                rb.velocity = direction * speed;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
            else
            {
                rb.velocity = transform.right * speed;
            }
        }
    }


    private Transform FindClosestEnemyInRadius()
    {
        Vector3 currentPos = transform.position;

        int count = Physics2D.OverlapCircleNonAlloc(currentPos, detectionRadius, detectionBuffer, enemyLayer);

        Transform closest = null;
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < count; i++)
        {
            Collider2D enemyCollider = detectionBuffer[i];

            if (enemyCollider == null) continue;

            float distance = Vector3.Distance(enemyCollider.transform.position, currentPos);
            if (distance < minDistance)
            {
                closest = enemyCollider.transform;
                minDistance = distance;
            }
        }

        return closest;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }
        PoolManager.Instance.ReturnToPool("AllyFire", gameObject);
    }

    protected IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(timeLimit);
        PoolManager.Instance.ReturnToPool("AllyFire", gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
