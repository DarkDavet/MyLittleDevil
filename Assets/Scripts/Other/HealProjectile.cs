using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HealProjectile : BaseProjectile
{
    private Transform _target;
    public float rotateSpeed = 200f;
    // private Vector2 direction;
    public override void OnObjectSpawn()
    {
        timer = StartCoroutine(ReturnToPoolAfterTime());
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null && enemyHealth.currentHealth < enemyHealth.maxHealth)
            {
                _target = enemy.transform;
                break;
            }
        }

        if (_target != null)
        {
            Vector2 direction = (_target.position - transform.position).normalized;
            rb.velocity = direction * speed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            Debug.LogWarning("No suitable target found.");
        }

    }

    void Update()
    {
        if (_target == null) return;

        // Move towards the target
        Vector2 direction = (Vector2)_target.position - (Vector2)transform.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        transform.Rotate(0, 0, -rotateAmount * rotateSpeed * Time.deltaTime);
        transform.position = Vector2.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }
        pool.ReturnToPool("EnemyHeal", gameObject);
    }
    protected IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(timeLimit);
        pool.ReturnToPool("EnemyHeal", gameObject);
    }
}
