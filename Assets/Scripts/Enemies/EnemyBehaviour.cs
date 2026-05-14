using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : BaseAIBehaviour
{
    public Transform player;
    public Transform minBorder;
    public Transform maxBorder;
    public EnemyShooting _attack;

    public float attackRange = 1f;
    public float minX = 0f;
    public float maxX = 4f;

    private Vector2 movementDirectionX = Vector2.left;

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < attackRange)
        {
            MovementY();
            MovementX();
            FireCalculate();
        }
    }

    private void MovementX()
    {
        if (player == null) return;

        float targetX = minBorder.position.x + targetPosition.x;
        Vector3 targetPos = new Vector3(targetX, transform.position.y, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - targetX) < 0.1f)
        {
            targetPosition.x = Random.Range(minX, maxX);
        }
    }

    public void FireCalculate()
    {
        if (_attack == null) return;

        if (Time.time >= nextTimeToFire)
        {
            float currentCooldown = Random.Range(_minfireRate, _maxfireRate);

            nextTimeToFire = Time.time + currentCooldown;

            _attack.Shoot();
        }
    }
}
    


