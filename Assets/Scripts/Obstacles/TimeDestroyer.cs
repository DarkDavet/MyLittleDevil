using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDestroyer : MonoBehaviour
{
    private float Timer = 0;

    private void Update()
    {
        Timer = Timer + 1 * Time.deltaTime;
        if (Timer > 2)
        {
            Destroy(gameObject);
        }
    }
}
