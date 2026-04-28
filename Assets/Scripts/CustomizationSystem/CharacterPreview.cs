using System.Collections.Generic;
using UnityEngine;

public class CharacterPreview : MonoBehaviour
{
    [System.Serializable]
    public struct VisualSlot
    {
        public CustomizationCategory category; // Тип (Hat, Glasses...)
        public SpriteRenderer renderer;        // Ссылка на объект-слот на герое
    }

    [SerializeField] private List<VisualSlot> slots;
    [SerializeField] private ItemDatabase database;

    private void Start()
    {
        ApplyAllSavedItems();
    }

    // Метод для "примерки" или надевания предмета
    public void ApplyItem(CustomizationItem item)
    {
        // Ищем слот, который соответствует категории предмета
        var slot = slots.Find(s => s.category == item.category);

        if (slot.renderer != null)
        {
            slot.renderer.sprite = item.visualSprite;
        }
        else
        {
            Debug.LogWarning($"Слот для категории {item.category} не настроен в инспекторе!");
        }
    }

    public void ApplyAllSavedItems()
    {
        foreach (var slot in slots)
        {
            string savedId = PlayerPrefs.GetString("Equipped_" + slot.category.ToString(), "");

            if (!string.IsNullOrEmpty(savedId))
            {
                CustomizationItem item = database.GetItemById(savedId);
                if (item != null)
                {
                    slot.renderer.sprite = item.visualSprite;
                    continue; // Переходим к следующему слоту
                }
            }

            // Если для этой категории ничего не сохранено — ОБЯЗАТЕЛЬНО очищаем слот
            slot.renderer.sprite = null;
        }
    }

    // Метод для снятия предмета (очистки слота)
    public void ClearSlot(CustomizationCategory category)
    {
        var slot = slots.Find(s => s.category == category);
        if (slot.renderer != null)
        {
            slot.renderer.sprite = null;
        }
    }
}