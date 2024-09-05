using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FreezeType
{
    FreezeRotation,
    FreezeMovement
}

public class FreezeObject : MonoBehaviour
{
    [SerializeField] private float freezeDuration = 2f;
    [SerializeField] private FreezeType freezeType;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ice"))
        {
            switch (freezeType)
            {
                case FreezeType.FreezeRotation:
                    StartCoroutine(FreezeRotation());
                    break;
                case FreezeType.FreezeMovement:
                    StartCoroutine(FreezeMovement());
                    break;
            }
        }
    }

    private IEnumerator FreezeRotation()
    {
        RotateObject spinScript = GetComponent<RotateObject>();
        if (spinScript != null)
        {
            spinScript.SetFrozen(true);
            yield return new WaitForSeconds(freezeDuration);
            spinScript.SetFrozen(false);
        }
    }

    private IEnumerator FreezeMovement()
    {
        BaseAIBehaviour behaviour = GetComponent<BaseAIBehaviour>();
        if (behaviour != null)
        {
            behaviour.speed /= 2;    // !!!!!!!!!
            yield return new WaitForSeconds(freezeDuration);
            behaviour.speed *= 2;
        }
    }
}
