using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerEffects : MonoBehaviour
    {

        [Header("Shield Visuals")]
        [SerializeField] private GameObject shieldVisualObject; 

        private Animator animator;
        private SpriteRenderer playerSpriteRenderer;
        private SpriteRenderer shieldSpriteRenderer;
        private Coroutine blinkCoroutine;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            playerSpriteRenderer = GetComponent<SpriteRenderer>();

            if (shieldVisualObject != null)
            {
                shieldVisualObject.SetActive(false);
                shieldSpriteRenderer = shieldVisualObject.GetComponent<SpriteRenderer>();
            }
        }

        // Включаем визуал щита и сбрасываем старое мигание, если оно было
        public void ActivateShieldVisual()
        {
            if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);

            if (shieldVisualObject != null) shieldVisualObject.SetActive(true);
            SetShieldAlpha(0.5f); 
        }

        // Выключаем щит полностью
        public void DeactivateShieldVisual()
        {
            if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);

            if (shieldVisualObject != null) shieldVisualObject.SetActive(false);
        }

        // Запуск мигания за X секунд до конца действия щита
        public void StartShieldBlinking(float timeUntilEnd)
        {
            if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
            blinkCoroutine = StartCoroutine(BlinkRoutine(timeUntilEnd));
        }

        private IEnumerator BlinkRoutine(float duration)
        {
            float elapsed = 0f;
            bool isVisible = true;

            while (elapsed < duration)
            {

                SetShieldAlpha(isVisible ? 0.3f : 0.5f);
                isVisible = !isVisible;

                float blinkSpeed = Mathf.Lerp(0.2f, 0.05f, elapsed / duration);

                yield return new WaitForSeconds(blinkSpeed);
                elapsed += blinkSpeed;
            }

            SetShieldAlpha(0.5f); 
        }

        private void SetShieldAlpha(float alpha)
        {
            if (shieldSpriteRenderer != null)
            {
                Color color = shieldSpriteRenderer.color;
                color.a = alpha;
                shieldSpriteRenderer.color = color;
            }
        }

        public void PlayHitAnimation()
        {
            if (animator != null) animator.SetTrigger("Hit");
        }
    }
}