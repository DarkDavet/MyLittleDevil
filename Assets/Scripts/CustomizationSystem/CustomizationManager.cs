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

    [SerializeField] private ItemDatabase itemDatabase;

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
            AchievementSystemCore.Instance.UpdateStandartProgress("unlock_2_custimized_items");
            currentSelectedItem.Unlock();

            ProcessPreviousItemUnequip(currentSelectedItem.category);
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

    private void ProcessPreviousItemUnequip(CustomizationCategory category)
    {
        // Смотрим в PlayerPrefs, одет ли какой-то ID в этой категории прямо сейчас
        string oldItemId = PlayerPrefs.GetString("Equipped_" + category.ToString(), "");

        if (!string.IsNullOrEmpty(oldItemId))
        {
            // Ищем этот предмет в нашем списке, чтобы вызвать его родной метод Unequip
            CustomizationItem oldItem = itemDatabase.allItems.Find(item => item.id == oldItemId);

            if (oldItem != null)
            {
                oldItem.Unequip(); 
            }
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
            OnEquipAction();
        }

        // Обновляем UI после переключения
        if (shopUI != null)
        {
            shopUI.RefreshShop();
        }
    }

    public void OnEquipAction()
    {
        if (currentSelectedItem == null || !currentSelectedItem.IsUnlocked) return;

        if (currentSelectedItem.IsEquipped) return;

        // снимаем предыдущий предмет этой же категории
        ProcessPreviousItemUnequip(currentSelectedItem.category);

        currentSelectedItem.Equip();
        previewer.ApplyItem(currentSelectedItem);

        if (shopUI != null)
        {
            shopUI.RefreshShop(); 
        }
    }

    public void ResetPreviewToEquippedItems()
    {
        // Очищаем текущий выбор, чтобы старый предмет не висел в памяти менеджера
        currentSelectedItem = null;

        // Возвращаем персонажу только те вещи, которые реально сохранены и надеты
        if (previewer != null)
        {
            previewer.ApplyAllSavedItems();
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