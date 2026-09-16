using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour, IHealable, IDamagable
{
    [SerializeField] public int maxHealth;
    
    [SerializeField] public int currentHealth;
    private float shieldDuration = 5f;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }

    private bool isImmortal = false;
    private Coroutine shieldCoroutine;

    private void Awake()
    {
        GameEvents.OnHealthHealed += Heal;
        GameEvents.OnHeroShieldActivated += ActivateShield;
    }

    private void Start()
    {
        maxHealth = 3;
        currentHealth = maxHealth;
        GameEvents.TriggerUpdatedPlayerHealth(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isImmortal)
        {
            Debug.Log("Урон заблокирован щитом!");
            return;
        }

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        GameEvents.TriggerUpdatedPlayerHealth(currentHealth);
        CheckDeadStatus();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        AudioManager.instance.PlaySfx("Heal");
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        GameEvents.TriggerUpdatedPlayerHealth(currentHealth);
        CheckDeadStatus();
    }

    private void ActivateShield()
    {
        // Если щит уже был активен, сбрасываем старый таймер, чтобы запустить новый
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }
        shieldCoroutine = StartCoroutine(ShieldDurationRoutine(shieldDuration));
    }

    private IEnumerator ShieldDurationRoutine(float duration)
    {
        isImmortal = true;
        Debug.Log("Щит активирован!");

        // Здесь можно включить визуальный эффект щита вокруг героя

        yield return new WaitForSeconds(duration);

        isImmortal = false;
        shieldCoroutine = null;
        Debug.Log("Щит отключен!");

        // Здесь отключаем визуальный эффект щита
    }

    private void CheckDeadStatus()  
    {
        if (currentHealth <= 0)
        {
            this.RequestState<LoseGameState>();
        }
    }


    private void OnDestroy()
    {
        GameEvents.OnHealthHealed -= Heal;
        GameEvents.OnHeroShieldActivated -= ActivateShield;
    }
}
