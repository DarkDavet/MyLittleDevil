using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shield Object", menuName = "Inventory System/Items/Shield")]
public class ShieldObject : ItemObject
{
    [SerializeField] private float shieldDuration = 5f; 

    public override void Use()
    {
        GameEvents.TriggerHeroShieldActivated(shieldDuration); 
        Debug.Log($"Щит активирован на {shieldDuration} сек.");
    }
}
