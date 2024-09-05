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
        if (TimeManager.Instance != null)
        {
            CoroutineHelper.Instance.StartHelperCoroutine(StoneEffect());
            Debug.Log("Slowing is working");
        }
        else
        {
            Debug.LogWarning("TimeManager not found!");
        }
    }

    private IEnumerator StoneEffect()
    {
        float originalSlowdownLength = TimeManager.Instance.slowdownLength;
        TimeManager.Instance.slowdownLength *= 2;
        TimeManager.Instance.TakeItSlow();
        yield return new WaitForSeconds(originalSlowdownLength * 2);
        TimeManager.Instance.slowdownLength = originalSlowdownLength;
    }
}
