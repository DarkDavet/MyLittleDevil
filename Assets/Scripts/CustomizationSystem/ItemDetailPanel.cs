using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailPanel : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image detailImage;
    [SerializeField] private TextMeshProUGUI detailTitle;
    [SerializeField] private TextMeshProUGUI detailDescription;
    [SerializeField] private TextMeshProUGUI detailPrice;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private GameObject buyPanel;
    [SerializeField] private GameObject equipPanel;

    private CustomizationManager customizationManager;

    private void Awake()
    {
        if (buyButton != null)
            buyButton.onClick.AddListener(OnBuyClick);
        if (equipButton != null)
            equipButton.onClick.AddListener(OnEquipClick);
    }

    private void Start()
    {
        HideDetails();
    }

    public void ShowItem(CustomizationItem item)
    {
        if (item == null) return;

        gameObject.SetActive(true);

        if (detailImage != null)
            detailImage.sprite = item.visualSprite;
        if (detailTitle != null)
            detailTitle.text = item.displayName;
        if (detailDescription != null)
            detailDescription.text = item.description;

        bool unlocked = item.IsUnlocked;

        buyPanel.SetActive(!unlocked);
        equipPanel.SetActive(unlocked);

        if (detailPrice != null)
        {
            detailPrice.text = unlocked ? "" : item.price.ToString();
        }
    }

    public void HideDetails()
    {
        gameObject.SetActive(false);
    }

    private void OnBuyClick()
    {
        if (customizationManager != null)
        {
            customizationManager.OnBuyButtonClick();
        }
    }

    private void OnEquipClick()
    {
        if (customizationManager != null)
        {
            customizationManager.ToggleEquipSelectedItem();
        }
    }

    public void UpdateButtons(CustomizationItem item)
    {
        if (item == null) return;

        if (buyPanel != null)
            buyPanel.SetActive(!item.IsUnlocked);
        if (equipPanel != null)
            equipPanel.SetActive(item.IsUnlocked);
    }
}