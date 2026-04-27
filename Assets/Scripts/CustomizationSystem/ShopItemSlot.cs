using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : MonoBehaviour
{
    [SerializeField] private CustomizationItem item;
    [Header("UI Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject priceTag;
    [SerializeField] private GameObject ownedTag;

    private CustomizationManager manager;

    public void Setup(CustomizationItem newItem, CustomizationManager shopManager)
    {
        item = newItem;
        manager = shopManager;
        icon.sprite = item.icon;
        RefreshState();
    }

    public void RefreshState()
    {
        bool unlocked = item.IsUnlocked;
        priceTag.SetActive(!unlocked);
        ownedTag.SetActive(unlocked);

        if (!unlocked)
            priceText.text = item.price.ToString();
    }

    public void OnClick()
    {
        // При клике сначала примеряем
        manager.SelectItem(item);

        // Если уже куплено — надеваем сразу
        if (item.IsUnlocked)
        {
            // Здесь можно вызвать метод надевания (Equip)
            Debug.Log("Предмет надет!");
        }
    }
}
