using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunGameState : GameState
{
    private GameStateController _stateController;
    public RunGameState(GameStateController stateController) : base(stateController)
    {
        _stateController = stateController;
    }
    public override void Enter()
    {
        Debug.Log($"RS activated");
        GameStateController.TimeReverseController.ResetHistory();
        GameStateController.CameraMoving.SetActive(true, 0.5f);
        DOVirtual.DelayedCall(0.5f, () => {
            GameStateController.TimeReverseController.ResetHistory();
        });
        TimeManager.Instance.TakeItSlow(1.5f);
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
