using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomInputController2 : MonoBehaviour
{
    protected EventSystem eventSystem;

    private void Start()
    {
        eventSystem = EventSystem.current;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            eventSystem.SetSelectedGameObject(null);
        }
    }
}
