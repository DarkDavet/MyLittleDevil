using AchievementSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyHealth : MonoBehaviour, IHealable, IDamagable
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    public int damage;  // WARNING!!!!!!!!!
    public GameObject explosionEffectPrefab;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Projectile hit detected. Applying damage.");
            TakeDamage(damage);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        FindObjectOfType<AudioManager>().PlaySfx("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        if (explosionEffect != null) Destroy(explosionEffect, 3f);
        Destroy(gameObject);
        FindObjectOfType<AudioManager>().PlaySfx("EnemyDeath");
        AchievementSystemCore.Instance.UpdateStandartProgress("defeat_2_angels");
    }

    public virtual void Heal(int heal)
    {
        currentHealth += heal;
    }
}
