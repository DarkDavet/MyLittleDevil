using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private GameStateContext gameStateManager;
    [SerializeField] private PoolManager poolManager;
    [SerializeField] private SceneData sceneData;
    [SerializeField] private InitialStateType initialStateType;

    private void Start()
    {
        if (sceneData != null && sceneData.collectibleStats != null)
        {
            CollectibleSystem.LevelStatsManager.Instance.InitializeStatsForLevel(sceneData);
        }
        gameStateManager.Init(initialStateType);
        this.RequestState<TutorialGameState>();
        poolManager.Init();
    }
}
