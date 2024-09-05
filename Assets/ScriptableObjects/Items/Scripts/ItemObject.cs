using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Time,
    Health,
    Default
}

public class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public int id;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;

    public virtual void Use()
    {

    }
}

    

