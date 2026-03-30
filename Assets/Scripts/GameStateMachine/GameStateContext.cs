using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateContext : MonoBehaviour
{
    private GameStateController stateController;
    public void Init()
    {
        stateController = new GameStateController();

        stateController.AddState(new RunGameState(stateController));

        stateController.SetState<RunGameState>();
    }

    private void Update()
    {
        stateController?.Update();
    }
}
