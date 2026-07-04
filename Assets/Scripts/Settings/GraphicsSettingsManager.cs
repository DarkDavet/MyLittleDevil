using System;
using UnityEngine;

/// <summary>
/// Manages graphics quality, VSync, frame rate, fullscreen, and resolution.
/// Resolution is stored as width/height/fullscreen JSON for portability across devices.
/// </summary>
public class GraphicsSettingsManager : MonoBehaviour, ISettingsSubsystem
{
    [Serializable]
    public struct ResolutionSave : IEquatable<ResolutionSave>
    {
        public int width;
        public int height;
        public bool fullscreen;

        public bool Equals(ResolutionSave other) =>
            width == other.width && height == other.height && fullscreen == other.fullscreen;

        public override bool Equals(object obj) => obj is ResolutionSave other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(width, height, fullscreen);
    }

    public event Action RefreshUIRequested;

    private const string QualityKey = "GraphicsQuality";
    private const string VSyncKey = "VSyncEnabled";
    private const string FramerateKey = "TargetFrameRate";
    private const string FullscreenKey = "FullscreenEnabled";
    private const string ResolutionKey = "ResolutionSave";

    private int _savedQualityLevel;
    private bool _savedVSync;
    private int _savedFrameRate;
    private bool _savedFullscreen;
    private ResolutionSave _savedResolution;

    private int _cachedQualityLevel;
    private int _cachedVSync; // vSyncCount: 0 or 1
    private int _cachedFrameRate;
    private bool _cachedFullscreen;
    private ResolutionSave _cachedResolution;

    public void Initialize()
    {
        if (PlayerPrefs.HasKey(QualityKey))
            _savedQualityLevel = PlayerPrefs.GetInt(QualityKey);

        if (PlayerPrefs.HasKey(VSyncKey))
            _savedVSync = PlayerPrefs.GetInt(VSyncKey) == 1;

        if (PlayerPrefs.HasKey(FramerateKey))
            _savedFrameRate = PlayerPrefs.GetInt(FramerateKey);

        if (PlayerPrefs.HasKey(FullscreenKey))
            _savedFullscreen = PlayerPrefs.GetInt(FullscreenKey) == 1;

        if (PlayerPrefs.HasKey(ResolutionKey))
        {
            var json = PlayerPrefs.GetString(ResolutionKey);
            _savedResolution = JsonUtility.FromJson<ResolutionSave>(json);
        }
        else
        {
            _savedResolution = new ResolutionSave
            {
                width = Screen.currentResolution.width,
                height = Screen.currentResolution.height,
                fullscreen = Screen.fullScreen
            };
        }

        ApplyAllSettings();
    }

    public void CacheCurrentState()
    {
        _cachedQualityLevel = _savedQualityLevel; 
        _cachedVSync = _savedVSync ? 1 : 0;
        _cachedFrameRate = _savedFrameRate;
        _cachedFullscreen = _savedFullscreen;
        _cachedResolution = _savedResolution;
    }

    public void ApplyAndSave()
    {
        QualitySettings.SetQualityLevel(_savedQualityLevel, true);
        QualitySettings.vSyncCount = _savedVSync ? 1 : 0;
        Application.targetFrameRate = _savedFrameRate;
        Screen.fullScreen = _savedFullscreen;
        ApplyResolution();

        PlayerPrefs.SetInt(QualityKey, _savedQualityLevel);
        PlayerPrefs.SetInt(VSyncKey, _savedVSync ? 1 : 0);
        PlayerPrefs.SetInt(FramerateKey, _savedFrameRate);
        PlayerPrefs.SetInt(FullscreenKey, _savedFullscreen ? 1 : 0);
        PlayerPrefs.SetString(ResolutionKey, JsonUtility.ToJson(_savedResolution));
        PlayerPrefs.Save();
    }

    public void DiscardChanges()
    {
        _savedQualityLevel = _cachedQualityLevel;
        _savedVSync = _cachedVSync == 1;
        _savedFrameRate = _cachedFrameRate;
        _savedFullscreen = _cachedFullscreen;
        _savedResolution = _cachedResolution;

        ApplyAllSettings();
        FireRefreshUI();
    }

    public void ResetToDefault()
    {
        int defaultQuality = Application.platform == RuntimePlatform.Android ? 2 : 5; // Medium or Ultra
        bool defaultVSync = Application.platform == RuntimePlatform.Android ? false : true;
        int defaultFrameRate = Application.platform == RuntimePlatform.Android ? -1 : 60;
        bool defaultFullscreen = Application.platform != RuntimePlatform.Android &&
                                 Application.platform != RuntimePlatform.IPhonePlayer;
        Resolution defaultResolution = Screen.currentResolution;

        _savedQualityLevel = defaultQuality;
        _savedVSync = defaultVSync;
        _savedFrameRate = defaultFrameRate;
        _savedFullscreen = defaultFullscreen;
        _savedResolution = new ResolutionSave
        {
            width = defaultResolution.width,
            height = defaultResolution.height,
            fullscreen = defaultFullscreen
        };

        ApplyAllSettings();
        FireRefreshUI();
    }

    public bool HasUnsavedChanges()
    {
        return GetQualityLevel() != _cachedQualityLevel ||
           GetVSync() != (_cachedVSync == 1) ||
           GetFrameRate() != _cachedFrameRate ||
           GetFullscreen() != _cachedFullscreen ||
           !_savedResolution.Equals(_cachedResolution);
    }

    // === Getters (used by UI) ===

    public int GetQualityLevel() => _savedQualityLevel;

    public bool GetVSync() => _savedVSync;

    public int GetFrameRate() => _savedFrameRate;

    public bool GetFullscreen() => _savedFullscreen;

    public Resolution GetCurrentResolution()
    {
        return Screen.currentResolution;
    }

    public Resolution[] GetAvailableResolutions()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            return new[] { Screen.currentResolution };

        return Screen.resolutions;
    }

    public string FormatResolution(Resolution resolution)
    {
        return $"{resolution.width}x{resolution.height}";
    }

    public string FormatResolutionWithFullscreen(Resolution resolution)
    {
        string suffix = _savedFullscreen ? " (Fullscreen)" : "";
        return FormatResolution(resolution) + suffix;
    }

    // === Setters (called by UI) ===

    public void SetQualityLevel(int level)
    {
        _savedQualityLevel = level;
    }

    public void SetVSync(bool vSync)
    {
        _savedVSync = vSync;
    }

    public void SetFrameRate(int frameRate)
    {
        _savedFrameRate = frameRate;
    }

    public void SetFullscreen(bool fullscreen)
    {
        _savedFullscreen = fullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution[] resolutions = GetAvailableResolutions();
        if (resolutionIndex >= 0 && resolutionIndex < resolutions.Length)
        {
            _savedResolution = new ResolutionSave
            {
                width = resolutions[resolutionIndex].width,
                height = resolutions[resolutionIndex].height,
                fullscreen = _savedFullscreen
            };
        }
    }

    // === Internal helpers ===

    private void ApplyAllSettings()
    {
        QualitySettings.SetQualityLevel(_savedQualityLevel, true);
        QualitySettings.vSyncCount = _savedVSync ? 1 : 0;
        Application.targetFrameRate = _savedFrameRate;
        //Screen.fullScreen = _savedFullscreen;     delete after testing
        ApplyResolution();
    }

    private void ApplyResolution()
    {
        Resolution[] resolutions = GetAvailableResolutions();
        FullScreenMode mode = _savedFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == _savedResolution.width &&
                resolutions[i].height == _savedResolution.height)
            {
                Screen.SetResolution(resolutions[i].width, resolutions[i].height, mode);
                return;
            }
        }

        // Resolution not found — fall back to native
        Resolution native = Screen.currentResolution;
        Screen.SetResolution(native.width, native.height, mode);

        _savedResolution = new ResolutionSave
        {
            width = native.width,
            height = native.height,
            fullscreen = _savedFullscreen
        };
    }

    public ResolutionSave GetCurrentResolutionSave()
    {
        Resolution current = Screen.currentResolution;
        return new ResolutionSave
        {
            width = current.width,
            height = current.height,
            fullscreen = _savedFullscreen
        };
    }

    protected void FireRefreshUI()
    {
        RefreshUIRequested?.Invoke();
    }
}
