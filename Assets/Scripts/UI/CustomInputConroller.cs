using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomInputConroller : MonoBehaviour
{
    protected EventSystem eventSystem;

    private void Start()
    {
        eventSystem = EventSystem.current;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            eventSystem.SetSelectedGameObject(null);
        }
    }
}
