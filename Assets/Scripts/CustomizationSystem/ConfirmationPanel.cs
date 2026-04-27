using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CustomizationShopUI shopUI;

    [Header("UI Elements")]
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    private void Awake()
    {
        // Панель должна быть выключена при старте
        gameObject.SetActive(false);

        if (confirmButton != null)
            confirmButton.onClick.AddListener(OnConfirm);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(HidePanel);
    }

    private void OnConfirm()
    {
        if (shopUI != null)
        {
            // Вызываем подтверждение через главный UI, 
            // чтобы он сам обновил баланс и сетку после покупки
            shopUI.ConfirmPurchase();
        }
        HidePanel();
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
}