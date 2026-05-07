using System.Collections;
using UnityEditor.EditorTools;
using UnityEngine;


public class BaseProjectile : MonoBehaviour, IPooledObject
{
    public float timeLimit = 2;
    public float speed = 10f;
    protected Coroutine timer;
    protected Rigidbody2D rb;

    public virtual void OnObjectSpawn()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = -transform.right * speed;
        
    }
}
