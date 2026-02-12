using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWidgetButton_Animator : MonoBehaviour
{
    public event Action OnAppearAnimationOverEvent;
    [SerializeField] private Animator animator;

    public void PlayHide()
    {
        this.animator.SetTrigger("hide");
    }

    private void Handle_AppearAnimationOver()
    {
        this.OnAppearAnimationOverEvent?.Invoke();
    }
}
