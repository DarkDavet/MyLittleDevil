using UnityEngine;
using System.Collections;

public class EnemyArrow: BaseProjectile
{
    private Transform _player;
    public override void OnObjectSpawn()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        if (rb != null)
        {
            timer = StartCoroutine(ReturnToPoolAfterTime());
            Vector2 direction = (_player.position - transform.position).normalized;
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
        pool.ReturnToPool("Arrow", gameObject);
    }
    protected IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(timeLimit);
        pool.ReturnToPool("Arrow", gameObject);
    }
}
