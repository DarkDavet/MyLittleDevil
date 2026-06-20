using AchievementSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;

    [SerializeField] private float startDelay = 2f;
    [SerializeField] private float endDelay = 2f;

    [SerializeField] private float speed = 100f; 
    [SerializeField] private float endPositionY = 1750f;

    private void Start()
    {
        StartCoroutine(CreditsSequence());
    }

    private IEnumerator CreditsSequence()
    {
        yield return new WaitForSeconds(startDelay);

        while (transform.localPosition.y < endPositionY)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(endDelay);

        UnlockFinishAchiev();
        sceneLoader.ReturnToMainMenu();
    }

    public void UnlockFinishAchiev()
    {
        AchievementSystemCore.Instance.UnlockAchievement("finish_the_game");
    }
}
