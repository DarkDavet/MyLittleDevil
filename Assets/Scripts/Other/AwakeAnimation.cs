using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    /*private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.keepAnimatorStateOnDisable = true;
    }*/

    private void OnEnable()
    {
        // Ensure the animation plays even when the game is paused
        _animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        // Manually play the animation state
        _animator.Play("Idle");
    }
}
