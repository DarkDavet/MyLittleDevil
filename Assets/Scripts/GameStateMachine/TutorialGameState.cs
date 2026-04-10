using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGameState : GameState
{
    private GameStateController _stateController;
    
    public TutorialGameState(GameStateController stateController) : base(stateController)
    {
        _stateController = stateController;
    }

    public override void Enter()
    {
        _stateController.UIManager.tutorialMenu.SetActive(true);
        _stateController.UIManager.pauseButton.gameObject.SetActive(false);
        Time.timeScale = 0;
        
        if (_stateController.TutorialSystem != null)
        {
            _stateController.TutorialSystem.OnTutorialFinished.AddListener(TutorialFinished);
            _stateController.TutorialSystem.StartTutorial();
        }
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
        _stateController.UIManager.tutorialMenu.SetActive(false);
        _stateController.UIManager.pauseButton.gameObject.SetActive(true);
        Time.timeScale = 1;
        
        if (_stateController.TutorialSystem != null)
        {
            _stateController.TutorialSystem.OnTutorialFinished.RemoveListener(TutorialFinished);
        }
    }

    private void TutorialFinished()
    {
        this.RequestPreviousState();
    }
}
