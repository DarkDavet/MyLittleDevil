using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectManager : MonoBehaviour
{
    [SerializeField] private Bar[] bar;
    private bool Toggle = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("Toggle is working");
            SwitchToggle();
        }
    }
    private void SwitchToggle()
    {
        Toggle = !Toggle;
        foreach (var obj in bar)
        {
            obj.SpriteUpdate();
        }     
    }

    public bool ShowToggle()
    {
        if (Toggle) return true;
        else return false;
    }
}
