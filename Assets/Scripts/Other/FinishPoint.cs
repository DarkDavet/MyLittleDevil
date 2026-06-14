using AchievementSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    [SerializeField] private string achiev_id; 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (achiev_id != null)
            {
                AchievementSystemCore.Instance.UnlockAchievement(achiev_id);
            }
            this.RequestState<WinGameState>();
        }
    }
}
