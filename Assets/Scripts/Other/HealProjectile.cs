using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HealProjectile : BaseProjectile
{
    public float rotateSpeed = 500f;

    private Transform _target;
    private int _healAmount;

    public void SetTarget(Transform target)
    {
        _target = target;

        if (_target != null)
        {
            Vector2 direction = (Vector2)_target.position - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    public void SetHealAmount(int amount) => _healAmount = amount;

    public override void OnObjectSpawn()
    {
        timer = StartCoroutine(ReturnToPoolAfterTime());
    }

    void Update()
    {
        if (_target == null)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            return;
        }

        // Логика поворота к цели
        Vector2 direction = (Vector2)_target.position - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Плавный поворот
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.TryGetComponent<IHealable>(out var health))
        {
            health.Heal(_healAmount);
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null; 
        }
        PoolManager.Instance.ReturnToPool("EnemyHeal", gameObject);
    }

    protected IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(timeLimit);
        ReturnToPool(); 
    }
}
