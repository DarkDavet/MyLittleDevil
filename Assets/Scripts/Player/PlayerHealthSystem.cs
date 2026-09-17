using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour, IHealable, IDamagable
{
    [SerializeField] public int maxHealth;
    
    [SerializeField] public int currentHealth;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }

    private bool isImmortal = false;
    public bool IsImmortal => isImmortal;
    private Coroutine shieldCoroutine;
    private PlayerEffects playerEffects;

    private void Awake()
    {
        playerEffects = GetComponent<PlayerEffects>();

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
        if (playerEffects != null) playerEffects.PlayHitAnimation();
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

    private void ActivateShield(float duration)
    {
        // Если щит уже был активен, сбрасываем старый таймер, чтобы запустить новый
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }
        shieldCoroutine = StartCoroutine(ShieldDurationRoutine(duration));
    }

    private IEnumerator ShieldDurationRoutine(float duration)
    {
        isImmortal = true;

        if (playerEffects != null)
        {
            playerEffects.ActivateShieldVisual(); 
        }

        float blinkWarningDuration = 1.5f;

        if (duration > blinkWarningDuration)
        {
            yield return new WaitForSeconds(duration - blinkWarningDuration);

            if (playerEffects != null)
            {
                playerEffects.StartShieldBlinking(blinkWarningDuration);
            }

            yield return new WaitForSeconds(blinkWarningDuration);
        }
        else
        {
            if (playerEffects != null) playerEffects.StartShieldBlinking(duration);
            yield return new WaitForSeconds(duration);
        }

        isImmortal = false;

        if (playerEffects != null)
        {
            playerEffects.DeactivateShieldVisual(); 
        }

        shieldCoroutine = null;

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
