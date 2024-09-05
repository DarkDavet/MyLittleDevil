using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : BaseEnemyHealth
{
    private DamageColorChange _damageColorChange;

    void Start()
    {
        currentHealth = maxHealth;
        _damageColorChange = GetComponent<DamageColorChange>();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.CompareTag("EnemyHeal"))
        {
            Heal(1);
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        _damageColorChange.ChangeDamageColor();
    }

    public override void Heal(int heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        _damageColorChange.ChangeDamageColor();
    }
}
