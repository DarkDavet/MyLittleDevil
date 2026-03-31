using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState
{
    protected readonly GameStateController GameStateController;
    public virtual bool IsFinalState => false;
    public GameState(GameStateController gameStateController)
    {
        GameStateController = gameStateController;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
