using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSettingsManager : MonoBehaviour, ISettingsSubsystem
{
    public InputActionAsset inputActions;
    [SerializeField] private List<RebindButton> rebindButtons = new List<RebindButton>();

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
        RefreshAllUIButtons();
    }

    public void ResetToDefault()
    {
        inputActions.RemoveAllBindingOverrides();
        RefreshAllUIButtons();
    }

    public bool HasUnsavedChanges()
    {
        // Сравниваем текущее состояние в меню с кэшем, созданным при входе
        return inputActions.SaveBindingOverridesAsJson() != _cachedJsonBindings;
    }

    public void RefreshAllUIButtons()
    {
        foreach (var btn in rebindButtons)
        {
            if (btn != null) btn.UpdateButtonText();
        }
    }
}
