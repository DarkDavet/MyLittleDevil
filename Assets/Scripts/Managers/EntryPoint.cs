using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private GameStateContext gameStateManager;

    private void Start()
    {
        gameStateManager.Init();
        this.RequestState<TutorialGameState>();
    }
}
