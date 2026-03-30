using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    [SerializeField] private float _speedOfCamera;
    private bool isCameraStop;
    public bool IsCameraStop {  get { return isCameraStop; } set { isCameraStop = value; } }

    private void Update()
    {
        if (!isCameraStop)
        {
            transform.position = new Vector3(transform.position.x + _speedOfCamera * Time.deltaTime, transform.position.y, transform.position.z);
        }
        
    }
}
