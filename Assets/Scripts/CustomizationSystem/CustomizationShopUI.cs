using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationShopUI : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject itemGridPanel;
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private GameObject noFundsPanel;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private Button closeButton;

    [Header("Child Components")]
    [SerializeField] private CategoryTabs categoryTabs;
    [SerializeField] private ItemGrid itemGrid;
    [SerializeField] private ItemDetailPanel detailPanelController;
    [SerializeField] private ConfirmationPanel confirmationPanelHandler;
    [SerializeField] private NoFundsPanel noFundsPanelHandler;

    private CustomizationManager customizationManager;
    private CustomizationItem selectedItem;

    private void Awake()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseShop);
    }

    private void Start()
    {
        UpdateBalance();
        SelectCategory(0);
    }

    public void OpenShop()
    {
        mainPanel.SetActive(true);
        SelectCategory(0);
        UpdateBalance();
    }

    public void CloseShop()
    {
        mainPanel.SetActive(false);
        selectedItem = null;
        if (detailPanelController != null)
            detailPanelController.HideDetails();
    }

    public void SelectCategory(int categoryIndex)
    {
        CustomizationCategory[] categories = new CustomizationCategory[] {
            CustomizationCategory.Hat,
            CustomizationCategory.Glasses,
            CustomizationCategory.Effect
        };

        if (categoryIndex >= 0 && categoryIndex < categories.Length)
        {
            if (itemGrid != null)
            {
                itemGrid.Populate(categories[categoryIndex]);
            }

            if (categoryTabs != null)
            {
                categoryTabs.SetCategoryCount(categories.Length);
            }
        }
    }

    public void OnItemClicked(CustomizationItem item)
    {
        selectedItem = item;

        if (customizationManager != null)
        {
            customizationManager.SelectItem(item);
        }

        if (detailPanelController != null)
        {
            detailPanelController.ShowItem(item);
        }
    }

    public void OnBuyClick()
    {
        if (customizationManager != null && selectedItem != null)
        {
            customizationManager.OnBuyButtonClick();
        }
    }

    public void ConfirmPurchase()
    {
        if (customizationManager != null)
        {
            customizationManager.ConfirmPurchase();
        }
        UpdateBalance();
    }

    public void ToggleEquip()
    {
        if (customizationManager != null && selectedItem != null)
        {
            customizationManager.ToggleEquipSelectedItem();
            if (detailPanelController != null)
            {
                detailPanelController.UpdateButtons(selectedItem);
            }
        }
    }

    public void OnConfirmPurchase()
    {
        if (confirmationPanelHandler != null)
            confirmationPanelHandler.ShowPanel();
    }

    private void UpdateBalance()
    {
        // TODO: Подставить реальный метод получения баланса
        if (balanceText != null)
        {
            // balanceText.text = "Balance: " + CollectibleManager.Instance.GetBalance().ToString();
            balanceText.text = "Balance: 0";
        }
    }

    public void ShowNoFunds()
    {
        if (noFundsPanelHandler != null)
            noFundsPanelHandler.ShowPanel();
    }

    private void RefreshShop()
    {
        if (itemGrid != null)
        {
            itemGrid.RefreshAll();
        }
        UpdateBalance();
    }
}