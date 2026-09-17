using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerEffects : MonoBehaviour
    {

        [Header("Shield Visuals")]
        [SerializeField] private GameObject shieldVisualObject; // Спрайт/эффект щита вокруг героя

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            // Если объект щита передан, изначально выключаем его
            if (shieldVisualObject != null) shieldVisualObject.SetActive(false);
        }

        // Включаем или выключаем визуальный щит
        public void SetShieldVisual(bool isActive)
        {
            if (shieldVisualObject != null)
            {
                shieldVisualObject.SetActive(isActive);
            }
        }

        // Проигрываем анимацию получения урона
        public void PlayHitAnimation()
        {
            if (animator != null)
            {
                animator.SetTrigger("Hit");
            }
        }
    }
}