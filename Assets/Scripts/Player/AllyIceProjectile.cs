using System.Collections;
using UnityEngine;

public class AllyIceProjectile : BaseProjectile
{
    private Transform enemy;

    public override void OnObjectSpawn()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        if (rb != null)
        {
            timer = StartCoroutine(ReturnToPoolAfterTime());
            Vector2 direction = (enemy.position - transform.position).normalized;
            rb.velocity = direction * speed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }
        pool.ReturnToPool("AllyIce", gameObject);
    }

    protected IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(timeLimit);
        pool.ReturnToPool("AllyIce", gameObject);
    }
}
