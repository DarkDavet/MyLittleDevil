using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar: MonoBehaviour, IHealthObserver
{
    public List<GameObject> hearts;

    private void Start()
    {
        PlayerHealthSystem.instance.AddObserver(this);
        UpdateHearts(PlayerHealthSystem.instance.currentHealth);
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

    private void OnDestroy()
    {
        PlayerHealthSystem.instance.RemoveObserver(this);
    }
}
