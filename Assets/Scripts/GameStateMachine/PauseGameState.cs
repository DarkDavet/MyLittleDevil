using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameState : GameState
{
    private GameStateController _stateController;
    public PauseGameState(GameStateController stateController) : base(stateController)
    {
        _stateController = stateController;
    }
    public override void Enter()
    {
        _stateController.UIManager.pauseMenu.SetActive(true);
        _stateController.UIManager.pauseButton.gameObject.SetActive(false);
        Time.timeScale = 0;
    }

    public override void Update()
    {
        if (Input.anyKeyDown)
        {
            this.RequestPreviousState();
        }
    }

    public override void Exit()
    {
        _stateController.UIManager.pauseMenu.SetActive(false);
        _stateController.UIManager.pauseButton.gameObject.SetActive(true);
        Time.timeScale = 1;
    }
}
