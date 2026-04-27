using UnityEngine;
using UnityEngine.UI;

public class NoFundsPanel : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private float autoCloseDelay = 3f; // 3 секунды обычно комфортнее для чтения

    private void Awake()
    {
        // На всякий случай выключаем при старте
        gameObject.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(HidePanel);
    }

    public void ShowPanel()
    {
        // Если панель уже была открыта, сбрасываем старый таймер
        CancelInvoke(nameof(HidePanel));

        gameObject.SetActive(true);

        if (autoCloseDelay > 0)
        {
            Invoke(nameof(HidePanel), autoCloseDelay);
        }
    }

    public void HidePanel()
    {
        CancelInvoke(nameof(HidePanel));
        gameObject.SetActive(false);
    }

    // Полезно добавить, если игрок закроет весь магазин не дожидаясь автозакрытия этой панели
    private void OnDisable()
    {
        CancelInvoke(nameof(HidePanel));
    }
}