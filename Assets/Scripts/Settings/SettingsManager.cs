using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Audio")]
    public AudioMixer audioMixer;

    [Header("Controls")]
    public InputSettingsManager inputSettings;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- ГРАФИКА ---
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    public void SetVSync(bool isVSync)
    {
        QualitySettings.vSyncCount = isVSync ? 1 : 0;
        PlayerPrefs.SetInt("VSync", isVSync ? 1 : 0);
    }

    // --- АУДИО ---
    public void SetVolume(string parameterName, float value)
    {
        // Переводим значение слайдера (0..1) в децибелы (-80..0)
        float dB = value > 0 ? Mathf.Log10(value) * 20 : -80f;
        audioMixer.SetFloat(parameterName, dB);
        PlayerPrefs.SetFloat(parameterName, value);
    }

    // --- ЗАГРУЗКА ---
    private void LoadSettings()
    {
        // Графика
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        Screen.fullScreen = isFullscreen;

        bool isVSync = PlayerPrefs.GetInt("VSync", 1) == 1;
        QualitySettings.vSyncCount = isVSync ? 1 : 0;

        // Аудио (Громкость по умолчанию: 0.75f)
        float master = PlayerPrefs.GetFloat("MasterVol", 0.75f);
        SetVolume("MasterVol", master);

        float music = PlayerPrefs.GetFloat("MusicVol", 0.75f);
        SetVolume("MusicVol", music);
    }
}
