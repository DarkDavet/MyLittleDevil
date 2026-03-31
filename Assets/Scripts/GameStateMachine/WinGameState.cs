using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGameState : GameState
{
    private GameStateController _stateController;
    public WinGameState(GameStateController stateController) : base(stateController)
    {
        _stateController = stateController;
    }

    public override void Enter()
    {
        _stateController.PlayerControls.Disable();
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        
    }
}
