using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWidgetButton : MonoBehaviour
{
    [SerializeField] private UIWidgetButton_Animator animator;
    [SerializeField] private float lifeTime = 3.0f;

    private void OnEnable()
    {
        animator.OnAppearAnimationOverEvent += OnAppearAnimationOver;
    }

    private void OnDisable()
    {
        animator.OnAppearAnimationOverEvent -= OnAppearAnimationOver;
    }

    private void OnAppearAnimationOver()
    {
        StartCoroutine(LifeRoutine());
    }

    private IEnumerator LifeRoutine()
    {
        yield return new WaitForSeconds(lifeTime);
        animator.PlayHide();
    }
}
