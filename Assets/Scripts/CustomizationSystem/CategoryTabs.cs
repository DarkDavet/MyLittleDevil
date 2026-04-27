using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CategoryTabs : MonoBehaviour
{
    [Header("Tab Buttons")]
    [SerializeField] private List<Button> tabButtons = new List<Button>();
    [SerializeField] private List<Toggle> tabToggles = new List<Toggle>();

    [Header("Tab Labels")]
    [SerializeField] private List<GameObject> tabLabels = new List<GameObject>();

    [Header("Content")]
    [SerializeField] private GameObject[] categoryPanels;

    private CustomizationManager customizationManager;
    private int activeCategoryIndex = 0;

    private void Awake()
    {
        foreach (var toggle in tabToggles)
        {
            toggle.onValueChanged.AddListener(OnTabToggled);
        }

        foreach (var button in tabButtons)
        {
            button.onClick.AddListener(OnTabButtonClicked);
        }
    }

    private void Start()
    {
        ShowCategory(0);
    }

    private void OnTabToggled(bool isOn)
    {
        if (!isOn) return;

        for (int i = 0; i < tabToggles.Count; i++)
        {
            if (tabToggles[i].isOn)
            {
                activeCategoryIndex = i;
                ShowCategory(i);
                break;
            }
        }
    }

    private void OnTabButtonClicked()
    {
        for (int i = 0; i < tabButtons.Count; i++)
        {
            if (tabButtons[i] == EventSystem.current.currentSelectedGameObject)
            {
                activeCategoryIndex = i;
                ActivateTab(i);
                ShowCategory(i);
                break;
            }
        }
    }

    private void ActivateTab(int index)
    {
        for (int i = 0; i < tabToggles.Count; i++)
        {
            tabToggles[i].isOn = (i == index);
        }

        for (int i = 0; i < tabLabels.Count; i++)
        {
            if (tabLabels[i] != null)
            {
                tabLabels[i].SetActive(i == index);
            }
        }
    }

    private void ShowCategory(int index)
    {
        for (int i = 0; i < categoryPanels.Length; i++)
        {
            if (categoryPanels[i] != null)
            {
                categoryPanels[i].SetActive(i == index);
            }
        }
    }

    public void SetCategoryCount(int count)
    {
        if (tabToggles.Count < count)
        {
            Debug.LogWarning("Not enough tab toggles assigned in inspector!");
            return;
        }

        List<CustomizationCategory> categories = new List<CustomizationCategory> { CustomizationCategory.Hat, CustomizationCategory.Glasses, CustomizationCategory.Effect };

        for (int i = 0; i < count; i++)
        {
            if (i < categories.Count)
            {
                ActivateTab(i);
            }
        }
    }
}