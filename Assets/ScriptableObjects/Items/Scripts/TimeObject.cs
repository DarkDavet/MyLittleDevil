using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "New Time Object", menuName = "Inventory System/Items/Time")]
public class TimeObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Time;
    }

    public override void Use()
    {
        TimeManager.Instance.TakeItSlowExceptPlayer(10f);
    }
}
