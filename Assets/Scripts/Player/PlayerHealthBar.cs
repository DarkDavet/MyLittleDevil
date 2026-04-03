using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar: MonoBehaviour
{
    public List<GameObject> hearts;

    private void OnEnable()
    {
        GameEvents.OnUpdatePlayerHealth += UpdateHearts;
    }
 

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].SetActive(true);
            }
            else
            {
                hearts[i].SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        GameEvents.OnUpdatePlayerHealth -= UpdateHearts;
    }


}
