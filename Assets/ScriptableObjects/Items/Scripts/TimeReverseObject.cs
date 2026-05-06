using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Time Reverse Object", menuName = "Inventory System/Items/TimeReverse")]
public class TimeReverseObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Time;
    }

    public override void Use()
    {
        GameEvents.TriggerTimeReverseActivated();
    }
}
