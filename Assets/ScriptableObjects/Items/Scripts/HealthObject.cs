using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Object", menuName = "Inventory System/Items/Health")]
public class HealthObject : ItemObject
{
    public int restoreHealthValue;
    public void Awake()
    {
        type = ItemType.Health;
    }

    public override void Use()
    {
        PlayerHealthSystem.instance.Heal(1);
        Debug.Log("Healing is working");
    }
}
