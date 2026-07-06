using System;
using UnityEngine;

/// <summary>
/// Audio settings subsystem — manages master, music, and SFX volume.
/// Saves to PlayerPrefs as floats (0–1 range). Fires RefreshUIRequested on discard/reset.
/// </summary>
public class AudioSettingsManager : MonoBehaviour, ISettingsSubsystem
{
    public event Action RefreshUIRequested;

    private const string MasterVolumeKey = "AudioMasterVolume";
    private const string MusicVolumeKey = "AudioMusicVolume";
    private const string SfxVolumeKey = "AudioSfxVolume";

    private const float DefaultMasterVolume = 1f;
    private const float DefaultMusicVolume = 1f;
    private const float DefaultSfxVolume = 1f;

    private float _savedMasterVolume;
    private float _savedMusicVolume;
    private float _savedSfxVolume;

    private float _cachedMasterVolume;
    private float _cachedMusicVolume;
    private float _cachedSfxVolume;

    public void Initialize()
    {
        _savedMasterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, DefaultMasterVolume);
        _savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, DefaultMusicVolume);
        _savedSfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, DefaultSfxVolume);

        ApplyAllSettings();
    }

    public void CacheCurrentState()
    {
        _cachedMasterVolume = _savedMasterVolume;
        _cachedMusicVolume = _savedMusicVolume;
        _cachedSfxVolume = _savedSfxVolume;
    }

    public void ApplyAndSave()
    {
        PlayerPrefs.SetFloat(MasterVolumeKey, _savedMasterVolume);
        PlayerPrefs.SetFloat(MusicVolumeKey, _savedMusicVolume);
        PlayerPrefs.SetFloat(SfxVolumeKey, _savedSfxVolume);
        PlayerPrefs.Save();

        ApplyAllSettings();
    }

    public void DiscardChanges()
    {
        _savedMasterVolume = _cachedMasterVolume;
        _savedMusicVolume = _cachedMusicVolume;
        _savedSfxVolume = _cachedSfxVolume;

        ApplyAllSettings();
        FireRefreshUI();
    }

    public void ResetToDefault()
    {
        _savedMasterVolume = DefaultMasterVolume;
        _savedMusicVolume = DefaultMusicVolume;
        _savedSfxVolume = DefaultSfxVolume;

        ApplyAllSettings();
        FireRefreshUI();
    }

    public bool HasUnsavedChanges()
    {
        return Mathf.Abs(_savedMasterVolume - _cachedMasterVolume) > 0.001f ||
               Mathf.Abs(_savedMusicVolume - _cachedMusicVolume) > 0.001f ||
               Mathf.Abs(_savedSfxVolume - _cachedSfxVolume) > 0.001f;
    }

    // === Getters (used by UI) ===

    public float GetMasterVolume() => _savedMasterVolume;

    public float GetMusicVolume() => _savedMusicVolume;

    public float GetSfxVolume() => _savedSfxVolume;

    // === Setters (called by UI) ===

    public void SetMasterVolume(float volume)
    {
        _savedMasterVolume = Mathf.Clamp01(volume);
    }

    public void SetMusicVolume(float volume)
    {
        _savedMusicVolume = Mathf.Clamp01(volume);
    }

    public void SetSfxVolume(float volume)
    {
        _savedSfxVolume = Mathf.Clamp01(volume);
    }

    // === Internal helpers ===

    private void ApplyAllSettings()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMasterVolume(_savedMasterVolume);
            AudioManager.instance.SetMusicVolume(_savedMusicVolume);
            AudioManager.instance.SetSfxVolume(_savedSfxVolume);
        }
    }

    protected void FireRefreshUI()
    {
        RefreshUIRequested?.Invoke();
    }
}
