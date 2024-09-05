using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAIBehaviour : MonoBehaviour
{
    [SerializeField] protected float minY = -4f;
    [SerializeField] protected float maxY = 4f;
    public float speed = 2f;   //!!!!!!!

    [SerializeField] protected float _maxfireRate = 3f;
    [SerializeField] protected float _minfireRate = 1f;

    protected float _fireRate;
    protected float nextTimeToFire = 0f;

    protected Vector2 movementDirectionY = Vector2.up;
    protected Vector2 targetPosition;

    protected void MovementY()
    {
        transform.Translate(movementDirectionY * speed * Time.deltaTime);

        if (transform.position.y >= maxY)
        {
            movementDirectionY = Vector2.down;
            targetPosition.y = Random.Range(minY, maxY);
        }
        else if (transform.position.y <= minY)
        {
            movementDirectionY = Vector2.up;
            targetPosition.y = Random.Range(minY, maxY);
        }

        if ((movementDirectionY == Vector2.up && transform.position.y >= targetPosition.y) ||
            (movementDirectionY == Vector2.down && transform.position.y <= targetPosition.y))
        {
            movementDirectionY *= -1;
            targetPosition.y = Random.Range(minY, maxY);
        }
    }
}
