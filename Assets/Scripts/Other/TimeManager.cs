using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;

    private bool _isRestoring = false; 

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Update()
    {
        if (!_isRestoring || Time.timeScale <= 0) return;

        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        if (Time.timeScale >= 1f) _isRestoring = false;
    }

    public void TakeItSlow(float customLength = -1)
    {
        if (customLength > 0) slowdownLength = customLength;

        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        _isRestoring = true;
    }

}
