using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    [SerializeField] private GameObject player;

    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;

    public bool IgnoreTimeScale { get; private set; }
    public bool IsSlowedDown => Time.timeScale < 0.99f;

    public float PlayerActiveTime { get; private set; }
    public float PlayerDeltaTime => IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
    public float PlayerFixedDeltaTime => IgnoreTimeScale ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime;

    private float _prePauseTimeScale = 1f;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            PlayerActiveTime += PlayerDeltaTime;
        }
        // Постепенно возвращаем время к 1.0
        if (Time.timeScale >= 1f || Time.timeScale <= 0) return;

        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        if (Time.timeScale >= 1f)
        {
            IgnoreTimeScale = false;
            SetPlayerAnimatorsMode(AnimatorUpdateMode.Normal);
        }
    }

    // РЕЖИМ 1: Замедляется абсолютно всё
    public void TakeItSlow(float length = -1)
    {
        if (length > 0) slowdownLength = length;
        IgnoreTimeScale = false;
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        SetPlayerAnimatorsMode(AnimatorUpdateMode.Normal);
    }

    // РЕЖИМ 2: Замедляется всё, кроме игрока и его систем
    public void TakeItSlowExceptPlayer(float length = -1)
    {
        TakeItSlow(length); // Сначала ставим общее замедление
        IgnoreTimeScale = true; // Но разрешаем игроку его игнорировать
        SetPlayerAnimatorsMode(AnimatorUpdateMode.UnscaledTime);
    }

    private void SetPlayerAnimatorsMode(AnimatorUpdateMode mode)
    {
        if (player == null) return;
        var animators = player.GetComponentsInChildren<Animator>();
        foreach (var anim in animators) anim.updateMode = mode;
    }

    public void Pause()
    {
        _prePauseTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = _prePauseTimeScale;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
