using System;
using UnityEngine;

public enum SoundType
{
    Music,
    SFX
}

/// <summary>
/// Singleton audio manager. Plays sounds by name and exposes master/music/SFX volume control.
/// Volume multipliers are applied on top of each Sound's base volume in Awake.
/// PlaySfx() and PlayMusic() enforce type correctness — a sound must match its method's type.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    [Header("Volume Multipliers (0–1)")]
    [Range(0f, 1f)] [SerializeField] private float _masterVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _musicVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _sfxVolume = 1f;

    [Header("Current Music")]
    [SerializeField] private Sound _currentMusicSound;

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

    // === Music playback helpers (used by MusicController) ===

    /// <summary>Returns the AudioSource for the currently playing music track.</summary>
    public AudioSource GetMusicSource()
    {
        if (_currentMusicSound != null && _currentMusicSound.source != null)
        {
            return _currentMusicSound.source;
        }
        return null;
    }

    /// <summary>Stops the currently playing music track.</summary>
    public void StopMusic()
    {
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

    /// <summary>
    /// Plays a music sound by name, tracking it as the current music source.
    /// Does not call Play() on the AudioSource — MusicController handles playback via fade-in.
    /// </summary>
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music sound: " + name + " not found!");
            return;
        }

        if (s.type != SoundType.Music)
        {
            Debug.LogWarning("AudioManager: sound '" + name + "' is not marked as Music type! Use PlaySfx() for SFX sounds.");
            return;
        }

        _currentMusicSound = s;
        ApplyMusicVolumeToSource(s);
    }

    private void ApplyMusicVolumeToSource(Sound s)
    {
        if (s.source != null)
        {
            s.source.volume = s.volume * _musicVolume * _masterVolume;
        }
    }

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

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    /// <summary>
    /// Plays a sound effect by name. The sound must be marked as SoundType.SFX.
    /// </summary>
    public void PlaySfx(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("SFX sound: " + name + " not found!");
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
}
