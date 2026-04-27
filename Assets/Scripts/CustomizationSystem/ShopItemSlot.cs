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
    [SerializeField] private GameObject equippedMark;

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
        bool equipped = item.IsEquipped;

        priceTag.SetActive(!unlocked);
        ownedTag.SetActive(unlocked && !equipped); // Показываем "Куплено", только если не надето
        equippedMark.SetActive(equipped);
    }

    public void OnClick()
    {
        manager.SelectItem(item);

        if (item.IsUnlocked)
        {
            // Если куплено — переключаем состояние (надеть/снять)
            manager.ToggleEquipSelectedItem();
        }
        else
        {
            // Если не куплено — просто примеряем
        }
    }
}
