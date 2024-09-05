using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    [SerializeField] private float _speedOfCamera;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x + _speedOfCamera * Time.deltaTime, transform.position.y, transform.position.z);
    }
}
