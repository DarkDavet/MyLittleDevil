using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : MonoBehaviour
{
    
    [Header("UI Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject priceTag;
    [SerializeField] private GameObject ownedTag;
    [SerializeField] private GameObject equippedMark;

    private CustomizationManager manager;
    private CustomizationShopUI shopUI;
    private CustomizationItem item;

    public void Setup(CustomizationItem newItem, CustomizationManager shopManager)
    {
        item = newItem;
        manager = shopManager;

        // Находим ShopUI, чтобы передавать клики
        shopUI = FindObjectOfType<CustomizationShopUI>();

        // Автоматически настраиваем кнопку на самом объекте слота
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(HandleClick);
        }

        icon.sprite = item.icon;
        if (priceText != null) priceText.text = item.price.ToString();

        RefreshState();
    }

    public void RefreshState()
    {
        if (item == null) return;

        bool unlocked = item.IsUnlocked;
        bool equipped = item.IsEquipped;

        if (priceTag != null) priceTag.SetActive(!unlocked);
        if (ownedTag != null) ownedTag.SetActive(unlocked && !equipped);
        if (equippedMark != null) equippedMark.SetActive(equipped);
    }

    private void HandleClick()
    {
        if (item == null) return;

        // 1. Уведомляем магазин, чтобы обновить панель деталей и примерку
        if (shopUI != null)
        {
            shopUI.OnItemClicked(item);
        }

        // 2. Если предмет уже куплен, его можно сразу надеть/снять кликом по слоту
        if (item.IsUnlocked && manager != null)
        {
            manager.ToggleEquipSelectedItem();
            RefreshState();
        }
    }
}
