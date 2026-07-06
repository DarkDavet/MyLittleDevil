using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum SoundType
{
    Music,
    SFX
}

/// <summary>
/// Singleton audio manager. Plays sounds by name and exposes master/music/SFX volume control.
/// Loads sounds from SoundPack ScriptableObjects at runtime.
/// PlaySfx() and PlayMusic() enforce type correctness — a sound must match its method's type.
/// Music fade transitions are handled internally by AudioManager (the only singleton).
/// </summary>
public class AudioManager : MonoBehaviour
{// === Fields ===

    [Header("Sound Packs")]
    public SoundPack[] soundPacks;

    [NonSerialized] public List<Sound> sounds = new List<Sound>();

    public static AudioManager instance;

    [Header("Volume Multipliers (0–1)")]
    [Range(0f, 1f)][SerializeField] private float _masterVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float _musicVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float _sfxVolume = 1f;

    [Header("Music Settings")]
    [SerializeField] private float _fadeDuration = 1.5f;
    [SerializeField] private float _minFadeVolume = 0.00f;

    [Header("Current Playback")]
    [SerializeField] private Sound _currentMusicSound;

    // Один выделенный источник для всей музыки в игре
    private AudioSource _musicSource;
    private Tween _musicFadeTween;

    // === Unity Lifecycle ===

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);

        FlattenPacks();
        CreateAudioSources();
    }

    private void FlattenPacks()
    {
        sounds.Clear();
        if (soundPacks == null) return;

        for (int i = 0; i < soundPacks.Length; i++)
        {
            if (soundPacks[i] != null && soundPacks[i].sounds != null)
            {
                sounds.AddRange(soundPacks[i].sounds);
            }
        }
    }

    private void CreateAudioSources()
    {
        // Создаем AudioSource только для SFX, чтобы не плодить их под каждый музыкальный трек
        foreach (Sound s in sounds)
        {
            if (s.type == SoundType.SFX)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }

        // Создаем один перманентный источник для проигрывания музыки
        _musicSource = gameObject.AddComponent<AudioSource>();
    }

    // === Getters ===
    public float GetMasterVolume() => _masterVolume;
    public float GetMusicVolume() => _musicVolume;
    public float GetSfxVolume() => _sfxVolume;

    // === Setters ===
    public void SetMasterVolume(float volume) { _masterVolume = Mathf.Clamp01(volume); UpdateMusicVolume(); }
    public void SetMusicVolume(float volume) { _musicVolume = Mathf.Clamp01(volume); UpdateMusicVolume(); }
    public void SetSfxVolume(float volume) => _sfxVolume = Mathf.Clamp01(volume);

    private void UpdateMusicVolume()
    {
        // Обновляем громкость только если музыка играет и сейчас не идет процесс затухания/появления
        if (_musicSource.isPlaying && _currentMusicSound != null && (_musicFadeTween == null || !_musicFadeTween.IsPlaying()))
        {
            _musicSource.volume = _currentMusicSound.volume * _musicVolume * _masterVolume;
        }
    }

    // === Music Playback ===

    public AudioSource GetMusicSource() => _musicSource;

    /// <summary>
    /// Plays a music track by name with a smooth crossfade transition.
    /// </summary>
    public void PlayMusic(string name)
    {
        Sound newMusic = sounds.Find(s => s.name == name);
        if (newMusic == null)
        {
            Debug.LogWarning($"AudioManager: music sound '{name}' not found!");
            return;
        }

        if (newMusic.type != SoundType.Music)
        {
            Debug.LogWarning($"AudioManager: sound '{name}' is not marked as Music type! Use PlaySfx() instead.");
            return;
        }

        // Если эта песня уже играет — ничего не делаем
        if (_currentMusicSound != null && _currentMusicSound.name == name) return;

        // Сбрасываем старый твин, если он выполнялся
        _musicFadeTween?.Kill();

        Sequence sequence = DOTween.Sequence();

        // 1. Если музыка уже играет — плавно тушим ее
        if (_musicSource.isPlaying && _currentMusicSound != null)
        {
            sequence.Append(DOTween.To(() => _musicSource.volume, v => _musicSource.volume = v, _minFadeVolume, _fadeDuration / 2f)
                .SetEase(Ease.OutQuad));

            sequence.AppendCallback(() => _musicSource.Stop());
        }

        // 2. Подготавливаем и запускаем новый трек
        sequence.AppendCallback(() =>
        {
            _currentMusicSound = newMusic;
            _musicSource.clip = newMusic.clip;
            _musicSource.pitch = newMusic.pitch;
            _musicSource.loop = newMusic.loop;
            _musicSource.volume = _minFadeVolume;
            _musicSource.Play();
        });

        // 3. Плавно разгоняем громкость нового трека
        float targetVolume = newMusic.volume * _musicVolume * _masterVolume;
        sequence.Append(DOTween.To(() => _musicSource.volume, v => _musicSource.volume = v, targetVolume, _fadeDuration / 2f)
            .SetEase(Ease.OutQuad));

        // Работает даже во время паузы игры (когда Time.timeScale = 0)
        sequence.SetUpdate(true);
        _musicFadeTween = sequence;
    }

    public void StopMusic()
    {
        _musicFadeTween?.Kill();
        _musicSource.Stop();
        _currentMusicSound = null;
    }

    public void PauseMusic() => _musicSource.Pause();
    public void ResumeMusic() => _musicSource.UnPause();

    // === SFX Playback ===

    public void PlaySfx(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s == null || s.type != SoundType.SFX) return;

        s.source.volume = s.volume * _sfxVolume * _masterVolume;
        s.source.Play();
    }
}
