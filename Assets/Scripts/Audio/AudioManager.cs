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
{
    // === Fields ===

    [Header("Sound Packs")]
    public SoundPack[] soundPacks;

    [NonSerialized] public List<Sound> sounds = new List<Sound>();

    public static AudioManager instance;

    [Header("Volume Multipliers (0–1)")]
    [Range(0f, 1f)] [SerializeField] private float _masterVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _musicVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _sfxVolume = 1f;

    [Header("Music")]
    [SerializeField] private Sound _currentMusicSound;
    [SerializeField] private float _fadeDuration = 1.5f;
    [SerializeField] private float _minFadeVolume = 0.01f;

    private Tween _fadeTween;

    // === Unity Lifecycle ===

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
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
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // === Getters (used by AudioSettingsManager and UI) ===

    public float GetMasterVolume() => _masterVolume;

    public float GetMusicVolume() => _musicVolume;

    public float GetSfxVolume() => _sfxVolume;

    // === Setters (called by AudioSettingsManager) ===

    public void SetMasterVolume(float volume)
    {
        _masterVolume = Mathf.Clamp01(volume);
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = Mathf.Clamp01(volume);
    }

    public void SetSfxVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
    }

    // === Music Playback ===

    /// <summary>Returns the AudioSource for the currently playing music track.</summary>
    public AudioSource GetMusicSource()
    {
        if (_currentMusicSound != null && _currentMusicSound.source != null)
        {
            return _currentMusicSound.source;
        }
        return null;
    }

    /// <summary>
    /// Plays a music track by name with fade transitions.
    /// If another track is playing, it fades out then the new one fades in.
    /// </summary>
    public void PlayMusic(string name)
    {
        Sound newMusic = sounds.Find(s => s.name == name);
        if (newMusic == null)
        {
            Debug.LogWarning("AudioManager: music sound '" + name + "' not found!");
            return;
        }

        if (newMusic.type != SoundType.Music)
        {
            Debug.LogWarning("AudioManager: sound '" + name + "' is not marked as Music type! Use PlaySfx() for SFX sounds.");
            return;
        }

        // If the same track is already playing, do nothing
        if (_currentMusicSound != null && _currentMusicSound.name == name)
        {
            return;
        }

        // If something is already playing, fade it out first
        if (_currentMusicSound != null)
        {
            FadeOutAndPlay(newMusic);
        }
        else
        {
            FadeIn(newMusic);
        }
    }

    /// <summary>Stops the currently playing music track immediately.</summary>
    public void StopMusic()
    {
        KillFadeTween();

        if (_currentMusicSound != null && _currentMusicSound.source != null)
        {
            _currentMusicSound.source.Stop();
            _currentMusicSound = null;
        }
    }

    /// <summary>Pauses the currently playing music track.</summary>
    public void PauseMusic()
    {
        if (_currentMusicSound != null && _currentMusicSound.source != null)
        {
            _currentMusicSound.source.Pause();
        }
    }

    /// <summary>Resumes a paused music track.</summary>
    public void ResumeMusic()
    {
        if (_currentMusicSound != null && _currentMusicSound.source != null)
        {
            _currentMusicSound.source.UnPause();
        }
    }

    // === SFX Playback ===

    /// <summary>
    /// Plays a sound effect by name. The sound must be marked as SoundType.SFX.
    /// </summary>
    public void PlaySfx(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("AudioManager: SFX sound '" + name + "' not found!");
            return;
        }

        if (s.type != SoundType.SFX)
        {
            Debug.LogWarning("AudioManager: sound '" + name + "' is not marked as SFX type! Use PlayMusic() for music sounds.");
            return;
        }

        float volume = s.volume * _sfxVolume * _masterVolume;
        s.source.volume = volume;
        s.source.Play();
    }

    // === Internal: Fade Logic ===

    private void FadeOutAndPlay(Sound newMusic)
    {
        KillFadeTween();

        AudioSource currentSource = _currentMusicSound.source;

        _fadeTween = DOTween.To(() => currentSource.volume, v => currentSource.volume = v, _minFadeVolume, _fadeDuration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                // Stop the old music
                currentSource.Stop();
                _currentMusicSound = null;

                // Start the new music with fade-in
                FadeIn(newMusic);
            });
    }

    private void FadeIn(Sound music)
    {
        KillFadeTween();

        _currentMusicSound = music;
        AudioSource source = music.source;

        // Set initial volume to minimum for fade-in
        source.volume = _minFadeVolume;
        source.pitch = music.pitch;
        source.loop = music.loop;

        // Fade to full music volume
        float targetVolume = music.volume * _musicVolume * _masterVolume;

        _fadeTween = DOTween.To(() => source.volume, v => source.volume = v, targetVolume, _fadeDuration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true)
            .OnStart(() =>
            {
                source.Play();
            });
    }

    private void KillFadeTween()
    {
        if (_fadeTween != null)
        {
            _fadeTween.Kill();
            _fadeTween = null;
        }
    }
}
