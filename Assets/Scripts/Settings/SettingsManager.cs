using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pure settings logic — manages subsystem lifecycle and persistence.
/// Contains no UI code. Attach this as DontDestroyOnLoad in a persistent scene.
/// UI interaction is handled by a separate SettingsWindow component.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Подсистемы настроек")]
    public InputSettingsManager inputSettings;
    public GraphicsSettingsManager graphicsSettings;
    public AudioSettingsManager audioSettings;

    private List<ISettingsSubsystem> _subsystems = new List<ISettingsSubsystem>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (inputSettings != null) _subsystems.Add(inputSettings);
            if (graphicsSettings != null) _subsystems.Add(graphicsSettings);
            if (audioSettings != null) _subsystems.Add(audioSettings);

            InitializeAll();
            CacheCurrentState();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CacheCurrentState()
    {
        foreach (var sub in _subsystems) sub.CacheCurrentState();
    }

    private void InitializeAll()
    {
        foreach (var sub in _subsystems) sub.Initialize();
    }

    // === PUBLIC LOGIC API — call these from UI ===

    /// <summary>Apply all changed settings and persist them to disk.</summary>
    public void ApplyAllSettings()
    {
        foreach (var sub in _subsystems) sub.ApplyAndSave();
        CacheCurrentState();
    }

    /// <summary>Reset all settings to their default values.</summary>
    public void ResetAllSettings()
    {
        foreach (var sub in _subsystems) sub.ResetToDefault();
    }

    /// <summary>Rollback all settings to the state they were in when the menu opened.</summary>
    public void DiscardAllSettings()
    {
        foreach (var sub in _subsystems) sub.DiscardChanges();
    }

    /// <summary>Returns true if any subsystem has unsaved changes since the last cache.</summary>
    public bool HasUnsavedChanges()
    {
        foreach (var sub in _subsystems)
        {
            if (sub.HasUnsavedChanges()) return true;
        }
        return false;
    }
}
