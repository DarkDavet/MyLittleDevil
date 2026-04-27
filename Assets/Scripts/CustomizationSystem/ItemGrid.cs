using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemGrid : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int itemsPerRow = 4;
    [SerializeField] private float verticalSpacing = 10f;
    [SerializeField] private float horizontalSpacing = 10f;

    [Header("References")]
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private CustomizationManager customizationManager;

    private List<ShopItemSlot> activeSlots = new List<ShopItemSlot>();
    private List<CustomizationItem> currentItems = new List<CustomizationItem>();

    public void Populate(CustomizationCategory category)
    {
        ClearItems();

        List<CustomizationItem> items = customizationManager.GetAllItems();
        items.RemoveAll(i => i.category != category);
        currentItems.AddRange(items);

        for (int i = 0; i < items.Count; i++)
        {
            GameObject slotObj = Instantiate(itemSlotPrefab, itemsContainer);
            slotObj.SetActive(true);

            ShopItemSlot slot = slotObj.GetComponent<ShopItemSlot>();
            if (slot != null)
            {
                slot.Setup(items[i], customizationManager);
                activeSlots.Add(slot);
            }
        }

        StartCoroutine(RepositionItems());
    }

    private System.Collections.IEnumerator RepositionItems()
    {
        yield return null;

        RectTransform containerRect = itemsContainer as RectTransform;
        float totalWidth = 0f;
        float totalHeight = 0f;

        for (int i = 0; i < activeSlots.Count; i++)
        {
            ShopItemSlot slot = activeSlots[i];
            RectTransform rect = slot.GetComponent<RectTransform>();

            int row = i / itemsPerRow;
            int col = i % itemsPerRow;

            float xPos = col * (rect.rect.width + horizontalSpacing);
            float yPos = -(row * (rect.rect.height + verticalSpacing));

            rect.anchoredPosition = new Vector2(xPos, yPos);

            if (col == itemsPerRow - 1 || i == activeSlots.Count - 1)
            {
                totalHeight = (row + 1) * (rect.rect.height + verticalSpacing);
            }
        }

        if (activeSlots.Count > 0)
        {
            RectTransform firstSlot = activeSlots[0].GetComponent<RectTransform>();
            totalWidth = itemsPerRow * (firstSlot.rect.width + horizontalSpacing);
        }

        containerRect.sizeDelta = new Vector2(totalWidth, totalHeight);
    }

    private void ClearItems()
    {
        foreach (ShopItemSlot slot in activeSlots)
        {
            if (slot != null && slot.gameObject != null)
            {
                Destroy(slot.gameObject);
            }
        }
        activeSlots.Clear();
        currentItems.Clear();
    }

    public void RefreshAll()
    {
        foreach (ShopItemSlot slot in activeSlots)
        {
            slot.RefreshState();
        }
    }

    public void OnItemScrolled(float scrollValue)
    {
        if (itemsContainer != null)
        {
            RectTransform rect = itemsContainer as RectTransform;
            rect.anchoredPosition = new Vector2(
                rect.anchoredPosition.x,
                Mathf.Clamp(rect.anchoredPosition.y + scrollValue, -1000f, 1000f)
            );
        }
    }
}