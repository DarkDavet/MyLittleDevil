using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private GameStateContext gameStateManager;
    [SerializeField] private TutorialSystem tutorialSystem;
    [SerializeField] private TutorialStorage tutorialStorage;

    private void Start()
    {
        gameStateManager.Init();
        tutorialSystem.Init(tutorialStorage);
        tutorialSystem.StartTutorial("tut_1");
    }
}
