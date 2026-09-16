using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shield Object", menuName = "Inventory System/Items/Shield")]
public class ShieldObject : ItemObject
{
    public override void Use()
    {
        GameEvents.TriggerHeroShieldActivated();
        Debug.Log("Предмет 'Щит' использован из инвентаря.");
    }
}
