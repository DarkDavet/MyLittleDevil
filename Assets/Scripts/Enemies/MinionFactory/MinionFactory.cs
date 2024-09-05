using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinionFactory : MonoBehaviour
{
    public abstract Minion CreateMinion(Vector2 position, Transform parent);
}
