using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseGameState : GameState
{
    private GameStateController _stateController;
    public override bool IsFinalState => true;
    public LoseGameState(GameStateController stateController) : base(stateController)
    {
        _stateController = stateController;
    }
    public override void Enter()
    {
        _stateController.UIManager.GameOverScreen.SetActive(true);
        _stateController.UIManager.VictoryScreen.SetActive(false);
        _stateController.Inventory.Clear();
    }

    public override void Update()
    {
       
    }

    public override void Exit()
    {
    }
}
