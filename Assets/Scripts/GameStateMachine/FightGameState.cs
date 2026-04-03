using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightGameState : GameState
{
    private GameStateController _stateController;
    public override bool IsFinalState => false;
    public FightGameState(GameStateController stateController) : base(stateController)
    {
        _stateController = stateController;
    }
    public override void Enter()
    {
        GameStateController.CameraMoving.SetActive(false, 0.8f);
        Debug.Log($"FS activated");
    }

    public override void Update()
    {
        _stateController.CameraMoving.MoveCamera();
    }

    public override void Exit()
    {

    }
}
