using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossHealth: BaseEnemyHealth
{
    [SerializeField] private Player _bird;
    [SerializeField] private GameObject _bossHud;
  
    [SerializeField] private HealthBar _healthBar;

    [SerializeField] GameObject _healthBarHeart;
    [SerializeField] GameObject _nimb;

    private Animator UIAnimator;
    private Animator nimbAnimator;


    private void Start()
    {
        currentHealth = maxHealth;
        UIAnimator = _healthBarHeart.GetComponent<Animator>();
        nimbAnimator = _nimb.GetComponent<Animator>();

        _healthBar.SetMaxHealth(maxHealth);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        _healthBar.SetHealth(currentHealth);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Projectile hit detected. Applying damage.");
            TakeDamage(damage);
            UIAnimator.SetTrigger("Hit");
            nimbAnimator.SetTrigger("Hit");
        }
    }
    
    public override void Die()
    {
        base.Die();
        Destroy(_bossHud);
        _bird.ChangeVictoryStatus();
    }
}
