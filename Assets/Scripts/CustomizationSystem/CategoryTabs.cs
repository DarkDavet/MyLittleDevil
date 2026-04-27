using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CategoryTabs : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CustomizationShopUI shopUI;

    [Header("Tab Toggles")]
    // Каждому Toggle в инспекторе соответствует индекс (0 = Hat, 1 = Glasses и т.д.)
    [SerializeField] private List<Toggle> tabToggles = new List<Toggle>();

    [Header("Visual Elements (Optional)")]
    // Если хочешь менять цвет текста или иконки при активации
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = Color.gray;

    private void Start()
    {
        // Настраиваем слушателей программно, чтобы не делать это руками в инспекторе
        for (int i = 0; i < tabToggles.Count; i++)
        {
            int index = i; // Локальная переменная для замыкания
            tabToggles[i].onValueChanged.AddListener((isOn) => {
                if (isOn) OnTabSelected(index);
            });
        }

        // Активируем первую вкладку по умолчанию
        if (tabToggles.Count > 0)
        {
            tabToggles[0].isOn = true;
            OnTabSelected(0);
        }
    }

    private void OnTabSelected(int index)
    {
        // 1. Уведомляем главный UI о смене категории
        if (shopUI != null)
        {
            shopUI.SelectCategory(index);
        }

        // 2. Обновляем визуальную часть (цвета текстов и т.д.)
        UpdateVisuals(index);
    }

    public void UpdateVisuals(int activeIndex)
    {
        for (int i = 0; i < tabToggles.Count; i++)
        {
            // Пример: меняем цвет текста внутри Toggle, если он есть
            var label = tabToggles[i].GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (label != null)
            {
                label.color = (i == activeIndex) ? activeColor : inactiveColor;
            }
        }
    }

    // Метод для внешней активации вкладки (например, из ShopUI)
    public void SetActiveTab(int index)
    {
        if (index >= 0 && index < tabToggles.Count)
        {
            tabToggles[index].isOn = true;
        }
    }
}