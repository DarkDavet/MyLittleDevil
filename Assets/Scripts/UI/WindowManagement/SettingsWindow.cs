using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI coordinator for the Settings screen.
/// Delegates all settings logic to SettingsManager and owns UI state (confirmation popup, window open/close).
/// Attach this to the Settings window prefab.
/// </summary>
public class SettingsWindow : UIWindow
{
    [Header("Logic")]
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private GraphicsSettingsManager graphicsSettings;

    [Header("UI - Graphics")]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle vSyncToggle;
    [SerializeField] private Button frameRate30Button;
    [SerializeField] private Button frameRate60Button;
    [SerializeField] private Button frameRateUnlockedButton;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    [Header("UI")]
    [SerializeField] private GameObject confirmationPopup;

    private void OnEnable()
    {
        if (graphicsSettings != null)
        {
            graphicsSettings.RefreshUIRequested += UpdateGraphicsUI;
        }
    }

    private void OnDisable()
    {
        if (graphicsSettings != null)
        {
            graphicsSettings.RefreshUIRequested -= UpdateGraphicsUI;
        }
    }

    private void UpdateGraphicsUI()
    {
        if (qualityDropdown != null)
        {
            qualityDropdown.value = graphicsSettings.GetQualityLevel();
        }

        if (vSyncToggle != null)
        {
            vSyncToggle.isOn = graphicsSettings.GetVSync();
        }

        if (frameRate30Button != null) frameRate30Button.interactable = graphicsSettings.GetFrameRate() != 30;
        if (frameRate60Button != null) frameRate60Button.interactable = graphicsSettings.GetFrameRate() != 60;
        if (frameRateUnlockedButton != null) frameRateUnlockedButton.interactable = graphicsSettings.GetFrameRate() != -1;

        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = graphicsSettings.GetFullscreen();
        }

        if (resolutionDropdown != null)
        {
            var resolutions = graphicsSettings.GetAvailableResolutions();
            var options = new List<TMP_Dropdown.OptionData>();
            for (int i = 0; i < resolutions.Length; i++)
            {
                options.Add(new TMP_Dropdown.OptionData(graphicsSettings.FormatResolutionWithFullscreen(resolutions[i])));
            }
            resolutionDropdown.options.Clear();
            resolutionDropdown.AddOptions(options);
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    resolutionDropdown.value = i;
                    break;
                }
            }
            resolutionDropdown.RefreshShownValue();
        }
    }

    // === UI Interaction Methods (called from UI buttons/toggles) ===

    public void SetQualityLevel(int level)
    {
        graphicsSettings?.SetQualityLevel(level);
    }

    public void SetVSync(bool isOn)
    {
        graphicsSettings?.SetVSync(isOn);
    }

    public void SetFrameRate30()
    {
        graphicsSettings?.SetFrameRate(30);
    }

    public void SetFrameRate60()
    {
        graphicsSettings?.SetFrameRate(60);
    }

    public void SetFrameRateUnlocked()
    {
        graphicsSettings?.SetFrameRate(-1);
    }

    public void SetFullscreen(bool isOn)
    {
        graphicsSettings?.SetFullscreen(isOn);
    }

    public void SetResolution(int index)
    {
        graphicsSettings?.SetResolution(index);
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        settingsManager?.CashCurrentState();
    }

    /// <summary>Called from UI "Apply" button.</summary>
    public void ApplySettings()
    {
        settingsManager?.ApplyAllSettings();
        CloseWindow();
    }

    /// <summary>Called from UI "Reset" button.</summary>
    public void ResetSettings()
    {
        settingsManager?.ResetAllSettings();
    }

    /// <summary>Called from UI "Close/Back" button — shows confirmation if there are unsaved changes.</summary>
    public void TryClose()
    {
        if (settingsManager != null && settingsManager.HasUnsavedChanges())
        {
            if (confirmationPopup != null)
            {
                confirmationPopup.SetActive(true);
            }
        }
        else
        {
            CloseWindow();
        }
    }

    /// <summary>Called from popup "Yes" button — discards changes and closes.</summary>
    public void ConfirmDiscard()
    {
        settingsManager?.DiscardAllSettings();
        if (confirmationPopup != null)
        {
            confirmationPopup.SetActive(false);
        }
        CloseWindow();
    }

    private void CloseWindow()
    {
        if (confirmationPopup != null)
        {
            confirmationPopup.SetActive(false);
        }
        UIWindowsManager.Instance.CloseCurrentWindow();
    }
}
