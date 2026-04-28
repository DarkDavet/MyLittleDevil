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

    [Header("Action Button")]
    [SerializeField] private Button actionButton; // Одна кнопка на всё
    [SerializeField] private TextMeshProUGUI actionButtonText;

    private CustomizationItem currentItem;

    private void Awake()
    {
        if (actionButton != null)
            actionButton.onClick.AddListener(OnActionButtonClick);
    }

    private void Start()
    {
        HideDetails();
    }

    public void ShowItem(CustomizationItem item)
    {
        if (item == null) return;
        currentItem = item;

        gameObject.SetActive(true);

        if (detailImage != null) detailImage.sprite = item.icon;
        if (detailTitle != null) detailTitle.text = item.displayName;
        if (detailDescription != null) detailDescription.text = item.description; 

        UpdateButtons(item);
    }

    private void OnActionButtonClick()
    {
        if (currentItem == null) return;

        if (!currentItem.IsUnlocked)
        {
            // Если не куплено — открываем окно подтверждения покупки
            shopUI.OnBuyClick();
        }
        else
        {
            // Если куплено — просто надеваем/снимаем сразу
            shopUI.ToggleEquip();
        }
    }

    public void UpdateButtons(CustomizationItem item)
    {
        if (item == null) return;

        bool unlocked = item.IsUnlocked;
        bool equipped = item.IsEquipped;

        // Обновляем цену (скрываем, если куплено)
        if (detailPrice != null)
        {
            detailPrice.text = unlocked ? "Owned" : $"{item.price}";
        }

        // Меняем текст на единственной кнопке
        if (actionButtonText != null)
        {
            if (!unlocked)
                actionButtonText.text = "Buy";
            else
                actionButtonText.text = equipped ? "Unequip" : "Equip";
        }
    }

    public void HideDetails()
    {
        gameObject.SetActive(false);
        currentItem = null;
    }
}