using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CategoryTabs : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CustomizationShopUI shopUI;

    [Header("Tab Buttons")]
    // Расположите кнопки в списке в порядке категорий: 0 - Hat, 1 - Glasses, 2 - Effect
    [SerializeField] private List<Button> tabButtons = new List<Button>();

    [Header("Visual Elements")]
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = Color.gray;
    [SerializeField] private float activeScale = 1.1f;

    private void Start()
    {
        // Настраиваем клики программно
        for (int i = 0; i < tabButtons.Count; i++)
        {
            int index = i;
            tabButtons[i].onClick.AddListener(() => OnTabButtonClicked(index));
        }

        // По умолчанию выбираем первую категорию
        if (tabButtons.Count > 0)
        {
            OnTabButtonClicked(0);
        }
    }

    private void OnTabButtonClicked(int index)
    {
        // 1. Уведомляем магазин
        if (shopUI != null)
        {
            shopUI.SelectCategory(index);
        }

        // 2. Обновляем визуал кнопок
        UpdateVisuals(index);
    }

    public void UpdateVisuals(int activeIndex)
    {
        for (int i = 0; i < tabButtons.Count; i++)
        {
            bool isActive = (i == activeIndex);

            // Меняем цвет текста
            var label = tabButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (label != null)
            {
                label.color = isActive ? activeColor : inactiveColor;
                // Опционально: делаем текст жирным для активной вкладки
                label.fontStyle = isActive ? FontStyles.Bold : FontStyles.Normal;
            }

            // Опционально: немного увеличиваем активную кнопку
            tabButtons[i].transform.localScale = isActive ? Vector3.one * activeScale : Vector3.one;

            // Опционально: выключаем кликабельность активной кнопки
            tabButtons[i].interactable = !isActive;
        }
    }
}