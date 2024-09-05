using System.Collections;
using UnityEditor.EditorTools;
using UnityEngine;


public class BaseProjectile : MonoBehaviour, IPooledObject
{
    public Rigidbody2D rb;
    public float timeLimit = 2;
    public float speed = 10f;
    protected PoolManager pool;
    protected Coroutine timer;

    protected void Start()
    {
        pool = PoolManager.Instance;
    }

    public virtual void OnObjectSpawn()
    {
        rb.velocity = - transform.right * speed;
    }

}
