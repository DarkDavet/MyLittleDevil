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
        CurrentHealth = MaxHealth;
        UIAnimator = _healthBarHeart.GetComponent<Animator>();
        if (_nimb != null) nimbAnimator = _nimb.GetComponent<Animator>();
        _healthBar.SetMaxHealth(MaxHealth);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        _healthBar.SetHealth(CurrentHealth);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Projectile hit detected. Applying damage.");
            TakeDamage(damage);
            UIAnimator.SetTrigger("Hit");
            if (nimbAnimator != null)  nimbAnimator.SetTrigger("Hit");
        }
    }
    
    public override void Die()
    {
        base.Die();
        Destroy(_bossHud);
        this.RequestState<WinGameState>();
    }
}
