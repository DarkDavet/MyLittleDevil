using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [System.Serializable]
    public struct VisualSlot
    {
        public CustomizationCategory category;
        public SpriteRenderer renderer;
    }

    [Header("References")]
    [SerializeField] private ItemDatabase database; // Наша центральная база
    [SerializeField] private List<VisualSlot> slots; // Настройки слотов (Hat -> SpriteRenderer и т.д.)

    private void Start()
    {
        ApplyAllSavedItems();
    }

    // Применяет конкретный предмет (вызывается из магазина при примерке)
    public void ApplyItem(CustomizationItem item)
    {
        if (item == null) return;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].category == item.category)
            {
                if (slots[i].renderer != null)
                    slots[i].renderer.sprite = item.visualSprite;
                return;
            }
        }
    }

    // Загружает всё, что наето на игрока, из PlayerPrefs
    public void ApplyAllSavedItems()
    {
        if (database == null)
        {
            Debug.LogError("ItemDatabase не назначена в PlayerVisuals на объекте " + gameObject.name);
            return;
        }

        // Мы проходим по категориям из Enum, чтобы загрузить всё по отдельности
        foreach (CustomizationCategory cat in System.Enum.GetValues(typeof(CustomizationCategory)))
        {
            string savedId = PlayerPrefs.GetString("Equipped_" + cat.ToString(), "");

            if (!string.IsNullOrEmpty(savedId))
            {
                CustomizationItem item = database.GetItemById(savedId);
                if (item != null)
                {
                    ApplyItem(item);
                }
            }
            else
            {
                // Если в этой категории ничего не надето, убедимся, что слот пуст
                ClearSlot(cat);
            }
        }
    }

    public void ClearSlot(CustomizationCategory category)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].category == category)
            {
                if (slots[i].renderer != null)
                    slots[i].renderer.sprite = null;
                return;
            }
        }
    }
}
