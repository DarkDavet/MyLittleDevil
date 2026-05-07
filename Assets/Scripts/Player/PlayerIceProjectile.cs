using System.Collections;
using UnityEngine;

/// <summary>
/// Represents an ice projectile fired by the player.
/// </summary>
public class PlayerIceProjectile : BaseProjectile
{
    private void FixedUpdate()
    {
        ApplyVelocity();
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
        PoolManager.Instance.ReturnToPool("Ice", gameObject);
    }

    /// <summary>
    /// Initializes the projectile velocity and starts the return timer.
    /// </summary>
    public override void OnObjectSpawn()
    {

        if (rb == null) rb = GetComponent<Rigidbody2D>();
        ApplyVelocity();
        if (timer != null) StopCoroutine(timer);
        timer = StartCoroutine(ReturnToPoolAfterTime());
    }
    /// <summary>
    /// Returns the projectile to the pool after a specified time.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    protected IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSecondsRealtime(timeLimit);
        PoolManager.Instance.ReturnToPool("Ice", gameObject);
    }

    private void ApplyVelocity()
    {
        float boost = 1f;
        // Снаряд летит быстро только если игрок активировал исключение из замедления
        if (TimeManager.Instance.IgnoreTimeScale && TimeManager.Instance.IsSlowedDown)
        {
            boost = 1f / Time.timeScale;
        }

        rb.velocity = transform.right * speed * boost;
    }
}
