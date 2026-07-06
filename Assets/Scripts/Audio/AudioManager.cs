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
/// </summary>
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    [Header("Volume Multipliers (0–1)")]
    [Range(0f, 1f)] [SerializeField] private float _masterVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _musicVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _sfxVolume = 1f;

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

    public void Play(string name)
    {
        Play(name, SoundType.SFX);
    }

    public void Play(string name, SoundType type)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        // Apply volume multipliers: base * type-specific * master
        float typeMultiplier = s.type == SoundType.Music ? _musicVolume : _sfxVolume;
        s.source.volume = s.volume * typeMultiplier * _masterVolume;

        s.source.Play();
    }
}
