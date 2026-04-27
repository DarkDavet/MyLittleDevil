using CollectibleSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationManager : MonoBehaviour
{
    [SerializeField] private CharacterPreview previewer;

    [Header("UI Panels")]
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private GameObject noFundsPanel;

    private CustomizationItem currentSelectedItem;

    public void SelectItem(CustomizationItem item)
    {
        currentSelectedItem = item;
        // Используем ApplyItem, чтобы предмет встал в свой слот (шапка на голову и т.д.)
        previewer.ApplyItem(item);
    }

    public void OnBuyButtonClick()
    {
        if (currentSelectedItem == null || currentSelectedItem.IsUnlocked) return;

        int playerBalance = CollectibleManager.Instance.GetItemCount(currentSelectedItem.currencyType.Id);

        if (playerBalance >= currentSelectedItem.price)
            confirmPanel.SetActive(true);
        else
            noFundsPanel.SetActive(true);
    }

    public void ConfirmPurchase()
    {
        if (currentSelectedItem == null) return;

        string currencyId = currentSelectedItem.currencyType.Id;
        int price = currentSelectedItem.price;

        if (CollectibleManager.Instance.SpendItem(currencyId, price))
        {
            currentSelectedItem.Unlock();
            currentSelectedItem.Equip(); // Сразу надеваем после покупки
            confirmPanel.SetActive(false);

            RefreshAllShopSlots();
            Debug.Log($"Куплено и надето: {currentSelectedItem.displayName}");
        }
        else
        {
            confirmPanel.SetActive(false);
            noFundsPanel.SetActive(true);
        }
    }

    public void ToggleEquipSelectedItem()
    {
        if (currentSelectedItem == null || !currentSelectedItem.IsUnlocked) return;

        if (currentSelectedItem.IsEquipped)
        {
            currentSelectedItem.Unequip();
            previewer.ClearSlot(currentSelectedItem.category);
        }
        else
        {
            currentSelectedItem.Equip();
            previewer.ApplyItem(currentSelectedItem);
        }

        RefreshAllShopSlots();
    }

    public void RefreshAllShopSlots()
    {
        foreach (var slot in FindObjectsOfType<ShopItemSlot>())
        {
            slot.RefreshState();
        }
    }

    public List<CustomizationItem> GetAllItems()
    {
        List<CustomizationItem> allItems = new List<CustomizationItem>();

        foreach (var asset in Resources.LoadAll<CustomizationItem>("Customization"))
        {
            allItems.Add(asset);
        }

        return allItems;
    }
}