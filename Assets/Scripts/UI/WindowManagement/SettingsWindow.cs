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

        InitQualityDropdown();
        InitResolutionDropdown();

        UpdateGraphicsUI();
    }

    private void OnDisable()
    {
        if (graphicsSettings != null)
        {
            graphicsSettings.RefreshUIRequested -= UpdateGraphicsUI;
        }
    }

    private void InitResolutionDropdown()
    {
        if (resolutionDropdown == null || graphicsSettings == null) return;

        var resolutions = graphicsSettings.GetAvailableResolutions();
        var options = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(new TMP_Dropdown.OptionData(graphicsSettings.FormatResolution(resolutions[i])));
        }

        resolutionDropdown.options.Clear();
        resolutionDropdown.AddOptions(options);
    }

    private void InitQualityDropdown()
    {
        if (qualityDropdown == null) return;

        qualityDropdown.options.Clear();

  
        List<string> options = new List<string>(QualitySettings.names);
        qualityDropdown.AddOptions(options);
    }

    private void UpdateGraphicsUI()
    {
        if (graphicsSettings == null) return;

        // 1. Качество
        if (qualityDropdown != null)
        {
            qualityDropdown.SetValueWithoutNotify(graphicsSettings.GetQualityLevel());
            qualityDropdown.RefreshShownValue();
        }

        // 2. Вертикальная синхронизация
        if (vSyncToggle != null)
        {
            vSyncToggle.SetIsOnWithoutNotify(graphicsSettings.GetVSync());
        }

        // 3. Фреймрейт (Кнопки)
        int currentFrameRate = graphicsSettings.GetFrameRate();
        if (frameRate30Button != null) frameRate30Button.interactable = currentFrameRate != 30;
        if (frameRate60Button != null) frameRate60Button.interactable = currentFrameRate != 60;
        if (frameRateUnlockedButton != null) frameRateUnlockedButton.interactable = currentFrameRate != -1;

        // 4. Полноэкранный режим
        if (fullscreenToggle != null)
        {
            fullscreenToggle.SetIsOnWithoutNotify(graphicsSettings.GetFullscreen());
        }

        // 5. Разрешение экрана
        if (resolutionDropdown != null)
        {
            InitResolutionDropdown();

            var resolutions = graphicsSettings.GetAvailableResolutions();
            var savedRes = graphicsSettings.GetSelectedResolutionSave();

            int targetIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == savedRes.width && resolutions[i].height == savedRes.height)
                {
                    targetIndex = i;
                    break;
                }
            }

            resolutionDropdown.SetValueWithoutNotify(-1);
            resolutionDropdown.SetValueWithoutNotify(targetIndex);
            resolutionDropdown.RefreshShownValue();
        }
    }

    // === UI Interaction Methods ===

    public void SetQualityLevel(int level)
    {
        graphicsSettings?.SetQualityLevel(level);
        UpdateGraphicsUI(); // Сразу обновляем интерфейс
    }

    public void SetVSync(bool isOn)
    {
        graphicsSettings?.SetVSync(isOn);
        UpdateGraphicsUI();
    }

    public void SetFrameRate30()
    {
        graphicsSettings?.SetFrameRate(30);
        UpdateGraphicsUI();
    }

    public void SetFrameRate60()
    {
        graphicsSettings?.SetFrameRate(60);
        UpdateGraphicsUI();
    }

    public void SetFrameRateUnlocked()
    {
        graphicsSettings?.SetFrameRate(-1);
        UpdateGraphicsUI();
    }

    public void SetFullscreen(bool isOn)
    {
        graphicsSettings?.SetFullscreen(isOn);
        UpdateGraphicsUI(); 
    }

    public void SetResolution(int index)
    {
        graphicsSettings?.SetResolution(index);
        UpdateGraphicsUI();
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        settingsManager?.CacheCurrentState(); 
        UpdateGraphicsUI();
    }

    public void ApplySettings()
    {
        settingsManager?.ApplyAllSettings();
        CloseWindow();
    }

    public void ResetSettings()
    {
        settingsManager?.ResetAllSettings();
        // Если ResetAllSettings внутри себя вызывает FireRefreshUI, 
        // то UpdateGraphicsUI вызовется автоматически по подписке.
    }

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

    public void ConfirmDiscard()
    {
        settingsManager?.DiscardAllSettings();
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
