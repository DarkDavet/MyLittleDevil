using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    private void Awake()
    {
        GameEvents.OnHealthHealed += Heal;
    }

    private void Start()
    {
        maxHealth = 3;
        currentHealth = maxHealth;
       
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        GameEvents.TriggerUpdatedPlayerHealth(currentHealth);
        CheckDeadStatus();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        FindObjectOfType<AudioManager>().Play("Heal");
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        GameEvents.TriggerHealthHealed(currentHealth);
    }

    private void CheckDeadStatus()  
    {
        if (currentHealth <= 0)
        {
            this.RequestState<LoseGameState>();
        }
    }
}
