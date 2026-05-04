using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : BaseEnemyHealth
{
    private DamageColorChange _damageColorChange;

    void Start()
    {
        CurrentHealth = MaxHealth;
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
        CurrentHealth += heal;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        _damageColorChange.ChangeDamageColor();
    }
}
