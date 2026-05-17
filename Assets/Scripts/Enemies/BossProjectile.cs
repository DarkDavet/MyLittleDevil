using UnityEngine;
using System.Collections;

public class BossProjectile : BaseProjectile
{
    [SerializeField] private string proj_name; 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }
        PoolManager.Instance.ReturnToPool(proj_name, gameObject);
    }

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        timer = StartCoroutine(ReturnToPoolAfterTime());
    }

    protected IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(timeLimit);
        PoolManager.Instance.ReturnToPool(proj_name, gameObject);
    }
}
