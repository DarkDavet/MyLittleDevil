using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlunger : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _height = 0.5f;
    [SerializeField] private bool _useSin = true;

    private float originalY;

    private void Start()
    {
        originalY = transform.position.y;
    }

    private void Update()
    {
        //float newY = Mathf.PingPong(Time.time *speed, height) + startPos.y;
        //transform.position = new Vector3(startPos.x, newY, 0);
        float newY;

        if (_useSin)
        {
            newY = originalY + _height * Mathf.Sin(Time.time * _speed);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
        else
        {
            newY = originalY + _height * Mathf.Cos(Time.time * _speed);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
        
    }
}
