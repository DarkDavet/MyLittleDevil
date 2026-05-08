using DG.Tweening;
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
        Debug.Log($"FS activated");
        GameStateController.CameraMoving.SetActive(false, 0.8f);
        if (_stateController.StatePrevious is PauseGameState)
        {
            TimeManager.Instance.TakeItSlow(1.5f);
            return;
        }
        GameStateController.TimeReverseController.ResetHistory();
        DOVirtual.DelayedCall(0.8f, () => {
            GameStateController.TimeReverseController.ResetHistory();
        });
    }

    public override void Update()
    {
        if (TimeReverseController.IsReversing) return;
        _stateController.CameraMoving.MoveCamera();
    }

    public override void Exit()
    {

    }
}
