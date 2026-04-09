using System.Collections;
using UnityEngine;

/// <summary>
/// Represents a fire projectile fired by ally units.
/// </summary>
public class AllyFireProjectile : BaseProjectile
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
        var enemyObj = GameObject.FindGameObjectWithTag("Enemy");
        if (enemyObj != null)
        {
            enemy = enemyObj.transform;
        }
        
        if (rb != null)
        {
            timer = StartCoroutine(ReturnToPoolAfterTime());
            if (enemy != null)
            {
                Vector2 direction = (enemy.position - transform.position).normalized;
                rb.velocity = direction * speed;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
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
        pool.ReturnToPool("AllyFire", gameObject);
    }
    /// <summary>
    /// Returns the projectile to the pool after a specified time.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    protected IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(timeLimit);
        pool.ReturnToPool("AllyFire", gameObject);
    }
}
