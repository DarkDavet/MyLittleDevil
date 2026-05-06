using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public bool IsTransitioning { get; private set; }
    [SerializeField] private float _speedOfCamera;

    private float _currentThrottle = 1f; 
    private Tween _stopTween;


    public void MoveCamera()
    {
        float step = _speedOfCamera * _currentThrottle * Time.deltaTime;
        transform.position += new Vector3(step, 0, 0);
    }

    public void SetActive(bool isActive, float duration = 0.5f)
    {
        if (_stopTween != null)
        {
            _stopTween.Kill();
        }

        IsTransitioning = true;
        float targetThrottle = isActive ? 1f : 0f;

        _stopTween = DOTween.To(() => _currentThrottle, x => _currentThrottle = x, targetThrottle, duration)
            .SetEase(Ease.OutQuad) 
            .SetUpdate(true)
            .OnComplete(() => IsTransitioning = false);
    }
}
