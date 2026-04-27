using CollectibleSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Customization/Item")]
public class CustomizationItem : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite visualSprite; 
    public Sprite icon;        

    public CollectibleType currencyType; 
    public int price;

    public bool IsUnlocked => PlayerPrefs.GetInt("Unlocked_" + id, 0) == 1;

    public void Unlock() => PlayerPrefs.SetInt("Unlocked_" + id, 1);
}