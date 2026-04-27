using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemGrid : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ItemDatabase database; // Наша база данных
    [SerializeField] private ShopItemSlot itemSlotPrefab;
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private CustomizationManager customizationManager;
    [SerializeField] private CustomizationShopUI shopUI; // Ссылка на главный UI

    private List<ShopItemSlot> activeSlots = new List<ShopItemSlot>();

    public void Populate(CustomizationCategory category)
    {
        ClearItems();

        if (database == null)
        {
            Debug.LogError("ItemDatabase не назначена в ItemGrid!");
            return;
        }

        // Фильтруем предметы из базы по категории
        foreach (var item in database.allItems)
        {
            if (item.category == category)
            {
                CreateSlot(item);
            }
        }
    }

    private void CreateSlot(CustomizationItem item)
    {
        ShopItemSlot newSlot = Instantiate(itemSlotPrefab, itemsContainer);
        newSlot.gameObject.SetActive(true);

        // Настраиваем ячейку
        newSlot.Setup(item, customizationManager);

        // Добавляем обработку клика, чтобы уведомлять главный UI
        Button btn = newSlot.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => shopUI.OnItemClicked(item));
        }

        activeSlots.Add(newSlot);
    }

    public void RefreshAll()
    {
        foreach (ShopItemSlot slot in activeSlots)
        {
            if (slot != null) slot.RefreshState();
        }
    }

    private void ClearItems()
    {
        foreach (ShopItemSlot slot in activeSlots)
        {
            if (slot != null) Destroy(slot.gameObject);
        }
        activeSlots.Clear();
    }
}