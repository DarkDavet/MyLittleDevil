using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateContext : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private InventoryObject inventory;
    [SerializeField] private CameraMoving cameraMoving;
    [SerializeField] private FireShooting fireShooting;
    [SerializeField] private IceShooting iceShooting;
    [SerializeField] private PlayerInputHandler playerInput;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private TutorialSystem tutorialSystem;

    private GameStateController _stateController;
    public void Init()
    {
        _stateController = new GameStateController();

        _stateController.Player = player;
        _stateController.Inventory = inventory;
        _stateController.CameraMoving = cameraMoving;
        _stateController.FireShooting = fireShooting;
        _stateController.IceShooting = iceShooting; 
        _stateController.PlayerInput = playerInput;
        _stateController.UIManager = uiManager;
        _stateController.SceneLoader = sceneLoader;
        _stateController.TutorialSystem = tutorialSystem;

        _stateController.AddState(new RunGameState(_stateController));
        _stateController.AddState(new FightGameState(_stateController));
        _stateController.AddState(new PauseGameState(_stateController));
        _stateController.AddState(new LoseGameState(_stateController));
        _stateController.AddState(new WinGameState(_stateController));
        _stateController.AddState(new TutorialGameState(_stateController));

        _stateController.SetState<RunGameState>();
    }

    private void Update() => _stateController?.Update();

    private void OnDestroy() => _stateController?.Dispose();
}
