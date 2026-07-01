using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Подсистемы настроек")]
    public InputSettingsManager inputSettings;

    [Header("UI Окна")]
    public GameObject confirmationPopup;

    private List<ISettingsSubsystem> _subsystems = new List<ISettingsSubsystem>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (inputSettings != null) _subsystems.Add(inputSettings);

            InitializeAll();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        foreach (var sub in _subsystems) sub.CacheCurrentState();
    }

    private void InitializeAll()
    {
        foreach (var sub in _subsystems) sub.Initialize();
    }


    // КНОПКА "ПРИМЕНИТЬ"
    public void ApplyAllSettings()
    {
        foreach (var sub in _subsystems) sub.ApplyAndSave();
        confirmationPopup.SetActive(false);
        UIWindowsManager.Instance.OpenWindow(WindowID.Main);
    }

    // КНОПКА "СБРОСИТЬ"
    public void ResetAllSettings()
    {
        foreach (var sub in _subsystems) sub.ResetToDefault();
    }

    // КНОПКА "НАЗАД / ЗАКРЫТЬ"
    public void TryCloseSettingsMenu()
    {
        if (HasAnyUnsavedChanges())
        {
            if (confirmationPopup != null) confirmationPopup.SetActive(true);
        }
        else
        {
            UIWindowsManager.Instance.OpenWindow(WindowID.Main);
        }
    }

    // ПОП-АП: КНОПКА "НЕТ" (ВЫЙТИ БЕЗ СОХРАНЕНИЯ)
    public void DiscardAndClose()
    {
        foreach (var sub in _subsystems) sub.DiscardChanges();
        if (confirmationPopup != null) confirmationPopup.SetActive(false);
        UIWindowsManager.Instance.OpenWindow(WindowID.Main);
    }

    private bool HasAnyUnsavedChanges()
    {
        foreach (var sub in _subsystems)
        {
            if (sub.HasUnsavedChanges()) return true; // Если хоть у одной подсистемы есть изменения
        }
        return false;
    }
}
