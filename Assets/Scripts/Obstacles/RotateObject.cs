using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private bool rotateOpposite = false;

    private bool isFrozen = false;

    private void Update()
    {
        if (!isFrozen)
        {
            float direction = rotateOpposite ? 1 : -1;
            transform.Rotate(0, 0, direction * _speed * Time.deltaTime);
        }
    }

    public void SetFrozen(bool frozen)
    {
        isFrozen = frozen;
    }
}
