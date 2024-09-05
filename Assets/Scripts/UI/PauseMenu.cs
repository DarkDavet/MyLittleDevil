using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu: MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _pauseButton;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            Resume();
        }
    }

    public void Pause()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        _pauseMenu.SetActive(false);
        _pauseButton.SetActive(true);
        Time.timeScale = 1; 
    }
}
