using System.Collections;
using UnityEngine;

public class PlayerFireProjectile : BaseProjectile
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }
        pool.ReturnToPool("Fire", gameObject);
    }

    public override void OnObjectSpawn()
    {
        rb.velocity = transform.right * speed;
        timer = StartCoroutine(ReturnToPoolAfterTime());
    }
    protected IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(timeLimit);
        pool.ReturnToPool("Fire", gameObject);
    }
}
