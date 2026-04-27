using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CustomizationShopUI shopUI;

    [Header("UI Elements")]
    [SerializeField] private Image detailImage;
    [SerializeField] private TextMeshProUGUI detailTitle;
    [SerializeField] private TextMeshProUGUI detailDescription;
    [SerializeField] private TextMeshProUGUI detailPrice;

    [Header("Buttons & Panels")]
    [SerializeField] private Button buyButton;
    [SerializeField] private Button toggleEquipButton; // Одна универсальная кнопка
    [SerializeField] private TextMeshProUGUI equipButtonText; // Текст "Equip" или "Unequip"

    [SerializeField] private GameObject buyPanel;
    [SerializeField] private GameObject equipPanel;

    private void Awake()
    {
        // Подписываемся на события главного UI
        if (buyButton != null)
            buyButton.onClick.AddListener(() => shopUI.OnBuyClick());

        if (toggleEquipButton != null)
            toggleEquipButton.onClick.AddListener(() => shopUI.ToggleEquip());
    }

    private void Start()
    {
        HideDetails();
    }

    public void ShowItem(CustomizationItem item)
    {
        if (item == null) return;

        gameObject.SetActive(true);

        if (detailImage != null) detailImage.sprite = item.icon; // Лучше показывать иконку
        if (detailTitle != null) detailTitle.text = item.displayName;

        // Описание (убедитесь, что в CustomizationItem есть поле description)
        if (detailDescription != null) detailDescription.text = item.displayName;

        UpdateButtons(item);
    }

    public void UpdateButtons(CustomizationItem item)
    {
        if (item == null) return;

        bool unlocked = item.IsUnlocked;
        bool equipped = item.IsEquipped;

        // Переключаем панели Купить / Надеть
        if (buyPanel != null) buyPanel.SetActive(!unlocked);
        if (equipPanel != null) equipPanel.SetActive(unlocked);

        // Обновляем цену
        if (detailPrice != null)
        {
            detailPrice.text = unlocked ? "Owned" : item.price.ToString();
        }

        // Обновляем текст кнопки надевания
        if (equipButtonText != null)
        {
            equipButtonText.text = equipped ? "Unequip" : "Equip";
        }
    }

    public void HideDetails()
    {
        gameObject.SetActive(false);
    }
}