using DG.Tweening;
using System;
using UnityEngine;

/// <summary>
/// Controls music playback with fade transitions.
/// Ensures only one music track plays at a time — when a new track is requested,
/// the current one fades out, then the new one fades in.
/// </summary>
public class MusicController : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private float _fadeDuration = 1.5f;
    [SerializeField] private float _minFadeVolume = 0.01f;

    private Sound _currentMusic;
    private Tween _fadeTween;

    // === Public API ===

    /// <summary>
    /// Play a music track by name. If another track is playing, it fades out first.
    /// </summary>
    public void PlayMusic(string name)
    {
        // If the same track is already playing, do nothing
        if (_currentMusic != null && _currentMusic.name == name)
        {
            return;
        }

        // Find the sound by name
        Sound newMusic = Array.Find(AudioManager.instance.sounds, s => s.name == name);
        if (newMusic == null)
        {
            Debug.LogWarning("MusicController: music sound '" + name + "' not found in AudioManager!");
            return;
        }

        // If something is already playing, fade it out first
        if (_currentMusic != null)
        {
            FadeOutAndPlay(newMusic);
        }
        else
        {
            FadeIn(newMusic);
        }
    }

    /// <summary>
    /// Stop any currently playing music immediately.
    /// </summary>
    public void StopMusic()
    {
        if (_fadeTween != null)
        {
            _fadeTween.Kill();
            _fadeTween = null;
        }

        if (AudioManager.instance != null)
        {
            AudioManager.instance.StopMusic();
        }

        _currentMusic = null;
    }

    /// <summary>
    /// Pause currently playing music.
    /// </summary>
    public void PauseMusic()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PauseMusic();
        }
    }

    /// <summary>
    /// Resume paused music.
    /// </summary>
    public void ResumeMusic()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ResumeMusic();
        }
    }

    /// <summary>
    /// Set music volume (0–1). Applies immediately.
    /// </summary>
    public void SetVolume(float volume)
    {
        if (AudioManager.instance == null) return;

        AudioManager.instance.SetMusicVolume(volume);

        // If music is playing, update the AudioSource volume directly
        AudioSource musicSource = AudioManager.instance.GetMusicSource();
        if (musicSource != null)
        {
            musicSource.volume = musicSource.volume; // volume is already set by AudioManager
        }
    }

    // === Internal fade logic ===

    private void FadeOutAndPlay(Sound newMusic)
    {
        if (_fadeTween != null)
        {
            _fadeTween.Kill();
        }

        AudioSource currentSource = _currentMusic?.source;

        _fadeTween = DOTween.To(() => currentSource.volume, v => currentSource.volume = v, _minFadeVolume, _fadeDuration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                // Stop the old music
                if (currentSource != null)
                {
                    currentSource.Stop();
                }

                AudioManager.instance.StopMusic();

                // Start the new music
                AudioManager.instance.PlayMusic(newMusic.name);
                FadeIn(newMusic);
            });
    }

    private void FadeIn(Sound music)
    {
        if (_fadeTween != null)
        {
            _fadeTween.Kill();
        }

        _currentMusic = music;
        AudioSource source = music.source;

        // Set initial volume to minimum for fade-in
        source.volume = _minFadeVolume;
        source.pitch = music.pitch;
        source.loop = music.loop;

        // Fade to full music volume
        float targetVolume = music.volume * AudioManager.instance.GetMusicVolume() * AudioManager.instance.GetMasterVolume();

        _fadeTween = DOTween.To(() => source.volume, v => source.volume = v, targetVolume, _fadeDuration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true)
            .OnStart(() =>
            {
                source.Play();
            });
    }
}
