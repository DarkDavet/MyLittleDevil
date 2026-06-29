using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSettingsManager : MonoBehaviour
{
    public InputActionAsset inputActions;
    public GameObject confirmationPopup;

    [SerializeField] private List<RebindButton> rebindButtons = new List<RebindButton>();

    private const string SaveKey = "CustomControlBindings";
    private string _workingJsonBindings;

    private void Awake()
    {
        LoadBindings();
    }

    public void SaveBindings()
    {
        string json = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    private void LoadBindings()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            inputActions.LoadBindingOverridesFromJson(json);
            _workingJsonBindings = json;
        }
        else
        {
            _workingJsonBindings = inputActions.SaveBindingOverridesAsJson();
        }
        inputActions.Enable();
    }

    public void ApplyAndSaveBindings()
    {
        // Берем текущее состояние из ассета (куда игрок накликал новые клавиши) и жестко пишем на диск
        _workingJsonBindings = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(SaveKey, _workingJsonBindings);
        PlayerPrefs.Save();
        Debug.Log("Настройки управления успешно применены и сохранены!");
    }

    // Вызывается, если игрок нажал "Отмена" или закрыл окно БЕЗ сохранения
    public void CancelAndDiscardChanges()
    {
        // Откатываем ассет к состоянию последнего сохранения (или дефолту)
        inputActions.RemoveAllBindingOverrides();
        if (!string.IsNullOrEmpty(_workingJsonBindings))
        {
            inputActions.LoadBindingOverridesFromJson(_workingJsonBindings);
        }
        RefreshAllUIButtons(); 
    }

    public void ResetToDefault()
    {
        inputActions.RemoveAllBindingOverrides();

        RefreshAllUIButtons();
        Debug.Log("Управление сброшено к дефолтному (нажмите Применить для сохранения)");
    }

    public bool HasUnsavedChanges()
    {
        string currentJson = inputActions.SaveBindingOverridesAsJson();
        return currentJson != _workingJsonBindings;
    }

    public void TryCloseSettingsMenu(GameObject settingsPanel)
    {
        if (HasUnsavedChanges())
        {
            if (confirmationPopup != null)
            {
                confirmationPopup.SetActive(true);
            }
        }
        else
        {
            settingsPanel.SetActive(false);
        }
    }

    public void RefreshAllUIButtons()
    {
        foreach (var btn in rebindButtons)
        {
            btn.UpdateButtonText();
        }
    }
}
