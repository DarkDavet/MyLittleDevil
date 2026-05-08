using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ally Object", menuName = "Inventory System/Items/IceAlly")]
public class IceAllyCallObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Ally;
    }

    public override void Use()
    {
        GameEvents.TriggerIceMinionSpawned();
        
    }
}
