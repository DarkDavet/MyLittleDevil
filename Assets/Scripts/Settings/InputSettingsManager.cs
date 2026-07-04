using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSettingsManager : MonoBehaviour, ISettingsSubsystem
{
    public InputActionAsset inputActions;

    /// <summary>Fired when binding state changes — UI should refresh displayed texts.</summary>
    public event Action RefreshUIRequested;

    private const string SaveKey = "CustomControlBindings";
    private string _savedJsonBindings;   // То, что реально на диске
    private string _cachedJsonBindings;  // Снимок при открытии меню

    public void Initialize()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            _savedJsonBindings = PlayerPrefs.GetString(SaveKey);
            inputActions.LoadBindingOverridesFromJson(_savedJsonBindings);
        }
        else
        {
            _savedJsonBindings = inputActions.SaveBindingOverridesAsJson();
        }
        inputActions.Enable();
    }

    public void CacheCurrentState()
    {
        _cachedJsonBindings = inputActions.SaveBindingOverridesAsJson();
    }

    public void ApplyAndSave()
    {
        _savedJsonBindings = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(SaveKey, _savedJsonBindings);
        PlayerPrefs.Save();
    }

    public void DiscardChanges()
    {
        inputActions.RemoveAllBindingOverrides();
        if (!string.IsNullOrEmpty(_cachedJsonBindings))
        {
            inputActions.LoadBindingOverridesFromJson(_cachedJsonBindings);
        }
        FireRefreshUI();
    }

    public void ResetToDefault()
    {
        inputActions.RemoveAllBindingOverrides();
        FireRefreshUI();
    }

    public bool HasUnsavedChanges()
    {
        // Сравниваем текущее состояние в меню с кэшем, созданным при входе
        return inputActions.SaveBindingOverridesAsJson() != _cachedJsonBindings;
    }

    /// <summary>Notify all UI listeners that binding state has changed.</summary>
    protected void FireRefreshUI()
    {
        RefreshUIRequested?.Invoke();
    }
}
