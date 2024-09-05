using System.Collections;
using UnityEngine;

public class PlayerIceProjectile : BaseProjectile
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }
        pool.ReturnToPool("Ice", gameObject);
    }

    public override void OnObjectSpawn()
    {
        rb.velocity = transform.right * speed;
        timer = StartCoroutine(ReturnToPoolAfterTime());
    }
    protected IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(timeLimit);
        pool.ReturnToPool("Ice", gameObject);
    }
}
