using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateContext : MonoBehaviour
{
    private GameStateController stateController;
    // Start is called before the first frame update
    private void Start()
    {
        stateController = new GameStateController();

        stateController.AddState(new RunGameState(stateController));
    }

    private void Update()
    {
        stateController?.Update();
    }
}
