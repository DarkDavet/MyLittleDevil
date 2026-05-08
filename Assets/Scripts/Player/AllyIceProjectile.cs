using System.Collections;
using UnityEngine;

/// <summary>
/// Represents an ice projectile fired by ally units.
/// </summary>
public class AllyIceProjectile : BaseProjectile
{
    /// <summary>
    /// Reference to the target enemy.
    /// </summary>
    private Transform enemy;

    /// <summary>
    /// Initializes the projectile and sets its direction towards the enemy.
    /// </summary>
    public override void OnObjectSpawn()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        var enemyObj = GameObject.FindGameObjectWithTag("Enemy");
        if (enemyObj != null)
        {
            enemy = enemyObj.transform;
        }

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

    /// <summary>
    /// Handles collision and returns the projectile to the pool.
    /// </summary>
    /// <param name="collision">Collision data.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }
        PoolManager.Instance.ReturnToPool("AllyIce", gameObject);
    }

    /// <summary>
    /// Returns the projectile to the pool after a specified time.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    protected IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(timeLimit);
        PoolManager.Instance.ReturnToPool("AllyIce", gameObject);
    }
}
