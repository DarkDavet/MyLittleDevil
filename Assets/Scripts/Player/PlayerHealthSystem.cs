using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    public static PlayerHealthSystem instance;
    public int maxHealth;
    public int currentHealth;
    private List<IHealthObserver> observers = new List<IHealthObserver>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        maxHealth = 3;
        currentHealth = maxHealth;
        NotifyObservers();
    }

    public void AddObserver(IHealthObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IHealthObserver observer)
    {
        observers.Remove(observer);
    }

    private void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.OnHealthChanged(currentHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        NotifyObservers();
        CheckGameOver();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        FindObjectOfType<AudioManager>().Play("Heal");
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        NotifyObservers();
    }

    private void CheckGameOver()   //WARNING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    {
        if (currentHealth <= 0)
        {
            Player.instance.ChangeGameOverStatus();
        }
    }
}
