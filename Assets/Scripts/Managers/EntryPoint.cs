using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private GameStateContext gameStateManager;
    [SerializeField] private PoolManager poolManager;
    [SerializeField] private SceneData sceneData;
    [SerializeField] private PlayerInputHandler playerInputHandler;
    [SerializeField] private InitialStateType initialStateType;

    [Header("Music")]
    [SerializeField] private string musicTrackId;

    private void Awake()
    {
        GameEvents.OnTutorialFinished += InitState;
    }
    private void Start()
    {
        playerInputHandler.DisablePlayerControls();

        if (!string.IsNullOrEmpty(musicTrackId))
        {
            AudioManager.instance.PlayMusic(musicTrackId);
        }

        if (sceneData != null && sceneData.collectibleStats != null)
        {
            CollectibleSystem.LevelStatsManager.Instance.InitializeStatsForLevel(sceneData);
        }
        gameStateManager.Init();
        this.RequestState<TutorialGameState>();
        poolManager.Init();
    }

    private void InitState()
    {
        gameStateManager.SetInitState(initialStateType);
        playerInputHandler.EnablePlayerControls();
    }

    private void OnDestroy()
    {
        GameEvents.OnTutorialFinished -= InitState;
    }
}
