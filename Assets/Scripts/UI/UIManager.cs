using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button pauseButton;
    public GameObject pauseMenu;


    public void Pause()
    {
        this.RequestState<PauseGameState>();
    }
}
