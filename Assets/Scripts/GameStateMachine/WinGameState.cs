using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGameState : GameState
{
    private GameStateController _stateController;
    public override bool IsFinalState => true;
    public WinGameState(GameStateController stateController) : base(stateController)
    {
        _stateController = stateController;
    }

    public override void Enter()
    {
        GameStateController.CameraMoving.SetActive(false, 0.8f);
        _stateController.PlayerInput.DisablePlayerControls();
        _stateController.SceneLoader.UnlockNewLevel();
        _stateController.UIManager.WinScreen.SetActive(true);
    }

    public override void Update()
    {
        _stateController.CameraMoving.MoveCamera();
    }

    public override void Exit()
    {
        
    }
}
