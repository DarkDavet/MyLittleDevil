using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject LooseScreen;
    public GameObject WinScreen;
    public GameObject PlayerHealthBar;
    public Button pauseButton;
    public GameObject pauseMenu;
    public GameObject tutorialMenu;


    public void Pause()
    {
        this.RequestState<PauseGameState>();
    }
}
