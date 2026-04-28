using CollectibleSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationShopUI : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private GameObject mainPanel;

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
        // Находим менеджер логики
        customizationManager = FindObjectOfType<CustomizationManager>();

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseShop);
    }

    private void Start()
    {
        // Инициализируем магазин при старте (если нужно сразу показать первую категорию)
        SelectCategory((int)CustomizationCategory.Hat);
        UpdateBalance();
    }

    public void OpenShop()
    {
        mainPanel.SetActive(true);
        UpdateBalance();
        // При открытии обновляем текущую сетку
        RefreshShop();
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
        // Приводим индекс напрямую к Enum
        CustomizationCategory selectedCat = (CustomizationCategory)categoryIndex;

        if (itemGrid != null)
        {
            itemGrid.Populate(selectedCat);
        }

        // Синхронизируем вкладки, если нужно (подсветка активной)
        if (categoryTabs != null)
        {
            categoryTabs.UpdateVisuals(categoryIndex);
        }
    }

    public void OnItemClicked(CustomizationItem item)
    {
        selectedItem = item;

        if (customizationManager != null)
        {
            customizationManager.SelectItem(item); // Примерка на персонаже
        }

        if (detailPanelController != null)
        {
            detailPanelController.ShowItem(item); // Показ инфо в панели
        }
    }

    // Вызывается из UI кнопки "Купить"
    public void OnBuyClick()
    {
        if (customizationManager != null && selectedItem != null)
        {
            customizationManager.OnBuyButtonClick();
        }
    }

    // Вызывается из модального окна ПОДТВЕРЖДЕНИЯ покупки
    public void ConfirmPurchase()
    {
        if (customizationManager != null)
        {
            customizationManager.ConfirmPurchase();

            // Обновляем панель деталей, чтобы кнопка стала "Unequip"
            if (selectedItem != null && detailPanelController != null)
            {
                detailPanelController.UpdateButtons(selectedItem);
            }
        }
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
            RefreshShop(); // Чтобы галочка в сетке обновилась
        }
    }

    public void UpdateBalance()
    {
        if (balanceText != null && selectedItem != null && selectedItem.currencyType != null)
        {
            // Берем баланс конкретной валюты, которая нужна для выбранного предмета
            int amount = CollectibleManager.Instance.GetItemCount(selectedItem.currencyType.Id);
            balanceText.text = $"{selectedItem.currencyType.DisplayName}: {amount}";
        }
        else if (balanceText != null)
        {
            balanceText.text = "Select an item";
        }
    }

    public void ShowNoFunds()
    {
        if (noFundsPanelHandler != null)
            noFundsPanelHandler.ShowPanel();
    }

    public void RefreshShop()
    {
        if (itemGrid != null)
        {
            itemGrid.RefreshAll();
        }
        UpdateBalance();
    }
}