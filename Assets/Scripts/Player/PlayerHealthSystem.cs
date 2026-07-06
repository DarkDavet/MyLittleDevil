using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour, IHealable
{
    /// <summary>
    /// Maximum health value for the player.
    /// </summary>
    [SerializeField] public int maxHealth;
    
    /// <summary>
    /// Current health value for the player.
    /// </summary>
    [SerializeField] public int currentHealth;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }

    /// <summary>
    /// Initializes event listeners.
    /// </summary>
    private void Awake()
    {
        GameEvents.OnHealthHealed += Heal;
    }

    /// <summary>
    /// Initializes health values and triggers health update event.
    /// </summary>
    private void Start()
    {
        maxHealth = 3;
        currentHealth = maxHealth;
        GameEvents.TriggerUpdatedPlayerHealth(currentHealth);
    }

    /// <summary>
    /// Reduces the player's health by the specified damage amount.
    /// </summary>
    /// <param name="damage">Amount of damage to apply.</param>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        GameEvents.TriggerUpdatedPlayerHealth(currentHealth);
        CheckDeadStatus();
    }

    /// <summary>
    /// Increases the player's health by the specified amount.
    /// </summary>
    /// <param name="amount">Amount of health to restore.</param>
    public void Heal(int amount)
    {
        currentHealth += amount;
        FindObjectOfType<AudioManager>().PlaySfx("Heal");
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        GameEvents.TriggerUpdatedPlayerHealth(currentHealth);
        CheckDeadStatus();
    }

    /// <summary>
    /// Checks if the player is dead and triggers the game over state if necessary.
    /// </summary>
    private void CheckDeadStatus()  
    {
        if (currentHealth <= 0)
        {
            this.RequestState<LoseGameState>();
        }
    }

    /// <summary>
    /// Removes event listeners when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        GameEvents.OnHealthHealed -= Heal;
    }
}
