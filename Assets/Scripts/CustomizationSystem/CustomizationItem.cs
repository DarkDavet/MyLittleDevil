using CollectibleSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum CustomizationCategory { Hat, Glasses, Effect }

[CreateAssetMenu(fileName = "NewItem", menuName = "Customization/Item")]
public class CustomizationItem : ScriptableObject
{
    public string id;
    public CustomizationCategory category;
    public string displayName;
    public string description;
    public Sprite visualSprite; 
    public Sprite icon;        

    public CollectibleType currencyType; 
    public int price;

    public bool IsUnlocked => PlayerPrefs.GetInt("Unlocked_" + id, 0) == 1;

    public void Unlock() => PlayerPrefs.SetInt("Unlocked_" + id, 1);

    public bool IsEquipped => PlayerPrefs.GetString("Equipped_" + this.category.ToString(), "") == id;

    public void Equip()
    {
        // Используем category.ToString(), чтобы сохранить предмет в нужный слот (Hat, Glasses и т.д.)
        PlayerPrefs.SetString("Equipped_" + this.category.ToString(), id);
        PlayerPrefs.Save();
    }

    public void Unequip()
    {
        // Очищаем по категории
        PlayerPrefs.DeleteKey("Equipped_" + this.category.ToString());
        PlayerPrefs.Save();
    }
}