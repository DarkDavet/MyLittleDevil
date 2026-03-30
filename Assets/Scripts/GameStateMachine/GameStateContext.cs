using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateContext : MonoBehaviour
{
    
    private GameStateController _stateController;
    public void Init()
    {
        _stateController = new GameStateController();

        _stateController.AddState(new RunGameState(_stateController));
        _stateController.AddState(new FightGameState(_stateController));
        _stateController.AddState(new PauseGameState(_stateController));
        _stateController.AddState(new DialogueGameState(_stateController));
        _stateController.AddState(new WinGameState(_stateController));

        _stateController.SetState<RunGameState>();
    }

    private void Update() => _stateController?.Update();

    private void OnDestroy() => _stateController?.Dispose();
}
