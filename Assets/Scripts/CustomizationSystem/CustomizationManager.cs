using AchievementSystem;
using CollectibleSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationManager : MonoBehaviour
{
    [SerializeField] private CharacterPreview previewer;
    [SerializeField] private CustomizationShopUI shopUI; // Ссылка на главный UI

    [Header("UI Panels")]
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private GameObject noFundsPanel;

    private CustomizationItem currentSelectedItem;

    public void SelectItem(CustomizationItem item)
    {
        currentSelectedItem = item;
        previewer.ApplyAllSavedItems();
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
            AchievementSystemCore.Instance.UpdateStandartProgress("unlock_3_custimized_items");
            currentSelectedItem.Unlock();
            currentSelectedItem.Equip(); // Логически надеваем

            // Визуально надеваем (чтобы предмет остался на герое после покупки)
            previewer.ApplyItem(currentSelectedItem);

            confirmPanel.SetActive(false);

            // ОБНОВЛЯЕМ ВЕСЬ UI
            if (shopUI != null)
            {
                shopUI.RefreshShop(); // Обновит сетку и баланс
                // Обновляем панель деталей, чтобы кнопка стала "Unequip"
                shopUI.OnItemClicked(currentSelectedItem);
            }

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

        // Обновляем UI после переключения
        if (shopUI != null)
        {
            shopUI.RefreshShop();
        }
    }

    public void RefreshAllShopSlots()
    {
        // Теперь этот метод можно заменить вызовом shopUI.RefreshShop(),
        // но оставим для совместимости
        foreach (var slot in FindObjectsOfType<ShopItemSlot>())
        {
            slot.RefreshState();
        }
    }
}