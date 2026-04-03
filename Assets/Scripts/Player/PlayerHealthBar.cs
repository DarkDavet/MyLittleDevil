using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar: MonoBehaviour
{
    public List<GameObject> hearts;

    private void Awake()
    {
        GameEvents.OnUpdatePlayerHealth += UpdateHearts;
        GameEvents.OnHealthHealed += UpdateHearts;
    }
    public void Init()
    {
        
    }
    public void OnHealthChanged(int currentHealth)
    {
        UpdateHearts(currentHealth);
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
}
