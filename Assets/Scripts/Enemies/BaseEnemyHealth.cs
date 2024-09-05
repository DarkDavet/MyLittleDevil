using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public int damage;  // WARNING!!!!!!!!!
    public GameObject explosionEffectPrefab;

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
        FindObjectOfType<AudioManager>().Play("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(explosionEffect, 3f);
        Destroy(gameObject);
        FindObjectOfType<AudioManager>().Play("EnemyDeath");
       // TimeManager.Instance.TakeItSlow();
    }

    public virtual void Heal(int heal)
    {
        currentHealth += heal;
    }
}
