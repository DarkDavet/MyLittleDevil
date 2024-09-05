using UnityEngine;
using System.Collections;

public class BossProjectile : BaseProjectile
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }
        pool.ReturnToPool("Lightning", gameObject);
    }

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        timer = StartCoroutine(ReturnToPoolAfterTime());
    }

    protected IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(timeLimit);
        pool.ReturnToPool("Lightning", gameObject);
    }
}
