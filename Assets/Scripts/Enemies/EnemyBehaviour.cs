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
        transform.Translate(movementDirectionX * speed * Time.deltaTime);
        Debug.Log("Enemy is moving!");

        float currentPosition = transform.position.x - minBorder.position.x;

        if (currentPosition <= minX)
        {
            Debug.Log("Enemy is moving right!");
            movementDirectionX = Vector2.right * 2;
            targetPosition.x = Random.Range(minX, maxX);
        }
        else if (currentPosition >= maxX)
        {
            Debug.Log("Enemy is moving left!");
            movementDirectionX = Vector2.left;
            targetPosition.x = Random.Range(minX, maxX);
        }

        if ((movementDirectionX == Vector2.right && currentPosition >= targetPosition.x) ||
            (movementDirectionX == Vector2.left && currentPosition <= targetPosition.x))
        {
            movementDirectionX *= -1;
            targetPosition.x = Random.Range(minX, maxX);
        }
    }

    public void FireCalculate()
    {
        if (Time.time >= nextTimeToFire)
        {
            _fireRate = Random.Range(_minfireRate, _maxfireRate);
            nextTimeToFire = Time.time + 1f / _fireRate;
            _attack.Shoot();
        }
    }
}
    


