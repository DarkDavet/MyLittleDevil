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

    [SerializeField] private List<VisualSlot> slots; // Настройте в инспекторе
    [SerializeField] private List<CustomizationItem> allItems; // Все ассеты предметов

    private void Start()
    {
        ApplyAllSavedItems();
    }

    public void ApplyItem(CustomizationItem item)
    {
        // Ищем нужный рендерер по категории предмета
        var slot = slots.Find(s => s.category == item.category);
        if (slot.renderer != null)
        {
            slot.renderer.sprite = item.visualSprite;
        }
    }

    public void ApplyAllSavedItems()
    {
        // Проходим по всем категориям и загружаем сохраненное
        foreach (var slot in slots)
        {
            string savedId = PlayerPrefs.GetString("Equipped_" + slot.category.ToString(), "");
            if (!string.IsNullOrEmpty(savedId))
            {
                CustomizationItem item = allItems.Find(x => x.id == savedId);
                if (item != null) slot.renderer.sprite = item.visualSprite;
            }
        }
    }
}
