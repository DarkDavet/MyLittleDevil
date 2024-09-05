using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Minion: BaseAIBehaviour
{
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected Transform _projectileSpawnPoint;
    protected Transform player;
    protected Transform minBorder;
    protected Transform maxBorder;
    protected Transform attackBorder;
    protected PoolManager pool;

    protected float detectionRadius = 10f;
    protected float minX = 0f;
    protected float maxX = 4f;

    protected Vector2 movementDirectionX = Vector2.left;

    protected void Awake()
    {
        pool = PoolManager.Instance;
        AttachObjects();
        player = GameObject.FindWithTag("Player").transform;
    }

    protected void Update()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(player.position, detectionRadius, enemyLayer);
        if (enemiesInRange.Length > 0)
        {
            Attack();
        }
        MovementY();
        Move();
        //float distanceToPlayer = Vector3.Distance(transform.position, enemy.position);
        //if (distanceToPlayer < attackRange)
    }

    public void Move()
    {
        transform.Translate(movementDirectionX * speed * Time.deltaTime);

        float currentPosition = transform.position.x - minBorder.position.x;

        if (currentPosition <= minX)
        {
            movementDirectionX = Vector2.right * 2;
            targetPosition.x = Random.Range(minX, maxX);
        }
        else if (currentPosition >= maxX)
        {
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

    private void AttachObjects()
    {
        GameObject MainCamera = GameObject.Find("Main Camera");
        if (MainCamera == null)
        {
            Debug.LogWarning("MainCamera not found.");
            return;
        }

        Transform Player = MainCamera.transform.Find("Player");
        if (Player == null)
        {
            Debug.LogWarning("Player not found under MainCamera.");
            return;
        }

        Transform StrangeObjects = Player.transform.Find("StrangeObjects");
        if (StrangeObjects == null)
        {
            Debug.LogWarning("StrangeObjects not found under Player.");
            return;
        }

        minBorder = StrangeObjects.transform.Find("minA");
        if (minBorder == null)
        {
            Debug.LogWarning("minA not found under StrangeObjects.");
        }

        maxBorder = StrangeObjects.transform.Find("maxA");
        if (maxBorder == null)
        {
            Debug.LogWarning("maxA not found under StrangeObjects.");
        }

        attackBorder = StrangeObjects.transform.Find("maxE");
        if ( attackBorder = null)
        {
            Debug.LogWarning("maxE not found under StrangeObjects");
        }
    }
    public abstract void Attack();
}
