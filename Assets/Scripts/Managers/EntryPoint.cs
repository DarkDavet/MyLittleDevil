using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private GameStateContext gameStateManager;
    [SerializeField] private SceneData sceneData;

    private void Start()
    {
        if (sceneData != null && sceneData.collectibleStats != null)
        {
            CollectibleSystem.LevelStatsManager.Instance.InitializeStatsForLevel(sceneData);
        }
        gameStateManager.Init();
        this.RequestState<TutorialGameState>();
    }
}
