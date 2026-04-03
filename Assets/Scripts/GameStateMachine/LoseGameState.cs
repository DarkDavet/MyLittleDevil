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
        if (_stateController.PlayerInput == null) Debug.LogError("??????: PlayerControls ?? ???????? ? ?????????!");
        if (_stateController.UIManager == null) Debug.LogError("??????: UIManager ?? ???????? ? ?????????!");
        if (_stateController.UIManager.LooseScreen == null) Debug.LogError("??????: ?????? LooseScreen ?? ???????? ?????? UIManager!");
        _stateController.PlayerInput.DisablePlayerControls();
        _stateController.UIManager.LooseScreen.SetActive(true);
        _stateController.Inventory.Clear();
    }

    public override void Update()
    {
       
    }

    public override void Exit()
    {
    }
}
