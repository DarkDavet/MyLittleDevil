using System.Collections;
using UnityEngine;

/// <summary>
/// Represents an ice projectile fired by the player.
/// </summary>
public class PlayerIceProjectile : BaseProjectile
{
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
        pool.ReturnToPool("Ice", gameObject);
    }

    /// <summary>
    /// Initializes the projectile velocity and starts the return timer.
    /// </summary>
    public override void OnObjectSpawn()
    {
        rb.velocity = transform.right * speed;
        timer = StartCoroutine(ReturnToPoolAfterTime());
    }
    /// <summary>
    /// Returns the projectile to the pool after a specified time.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    protected IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(timeLimit);
        pool.ReturnToPool("Ice", gameObject);
    }
}
