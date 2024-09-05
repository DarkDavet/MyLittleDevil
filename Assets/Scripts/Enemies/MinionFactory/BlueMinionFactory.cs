using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlueMinionFactory : MinionFactory
{
    public override Minion CreateMinion(Vector2 position, Transform parent)
    {
        GameObject blueMinion = GameObject.Instantiate(Resources.Load("BlueMinion"), position, Quaternion.identity, parent) as GameObject;
        return blueMinion.GetComponent<Minion>();
    }
}
