using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindButton : MonoBehaviour
{
    [Header("Настройки Action")]
    public InputActionReference actionReference;

    [Header("UI Элементы")]
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI actionNameText;

    private Button rebindButton;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    private void Awake()
    {
        rebindButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (actionReference != null)
        {
            if (actionNameText != null)
                actionNameText.text = actionReference.action.name;

            UpdateButtonText();
        }
        if (rebindButton != null)
        {
            rebindButton.onClick.AddListener(StartRebinding);
        }

        if (SettingsManager.Instance != null)
            SettingsManager.Instance.inputSettings.RefreshUIRequested += UpdateButtonText;
    }

    private void OnDisable()
    {
        rebindButton.onClick.RemoveListener(StartRebinding);

        if (SettingsManager.Instance != null)
            SettingsManager.Instance.inputSettings.RefreshUIRequested -= UpdateButtonText;
    }

    public void UpdateButtonText()
    {
        // Получаем читаемое имя клавиши (например, "Space" или "Left Click")
        string displayString = actionReference.action.GetBindingDisplayString();
        buttonText.text = displayString;
    }

    private void StartRebinding()
    {
        buttonText.text = "..."; // Показываем игроку, что игра ждет нажатия
        rebindButton.interactable = false;

        // Отключаем экшены на время ребинда, чтобы персонаж не прыгал/стрелял в меню
        actionReference.action.actionMap.Disable();

        rebindingOperation = actionReference.action.PerformInteractiveRebinding()
            // Исключаем случайное назначение мышки на кнопки движения (опционально)
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Pointer>/delta")
            // Событие при успешном нажатии клавиши
            .OnComplete(operation => FinishRebinding())
            // Событие при отмене (например, если игрок нажал Esc)
            .OnCancel(operation => FinishRebinding())
            .Start();
    }

    private void FinishRebinding()
    {
        // Возвращаем экшены обратно в рабочее состояние
        actionReference.action.actionMap.Enable();

        // Освобождаем память операции
        rebindingOperation.Dispose();

        rebindButton.interactable = true;
        UpdateButtonText();
    }
}
