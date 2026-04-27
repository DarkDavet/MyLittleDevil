using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPanel : MonoBehaviour
{
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private CustomizationManager customizationManager;

    private void Awake()
    {
        gameObject.SetActive(false);

        if (confirmButton != null)
            confirmButton.onClick.AddListener(OnConfirm);
        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancel);
    }

    private void OnConfirm()
    {
        if (customizationManager != null)
        {
            customizationManager.ConfirmPurchase();
        }
        gameObject.SetActive(false);
    }

    private void OnCancel()
    {
        gameObject.SetActive(false);
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