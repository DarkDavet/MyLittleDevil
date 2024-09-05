using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RedMinionFactory : MinionFactory
{
    public override Minion CreateMinion(Vector2 position, Transform parent)
    {
        GameObject redMinion = GameObject.Instantiate(Resources.Load("RedMinion"), position, Quaternion.identity, parent) as GameObject;
        return redMinion.GetComponent<Minion>();
    }
}
