using UnityEngine;

/// <summary>
/// UI coordinator for the Settings screen.
/// Delegates all settings logic to SettingsManager and owns UI state (confirmation popup, window open/close).
/// Attach this to the Settings window prefab.
/// </summary>
public class SettingsWindow : UIWindow
{
    [Header("Logic")]
    [SerializeField] private SettingsManager settingsManager;

    [Header("UI")]
    [SerializeField] private GameObject confirmationPopup;

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
