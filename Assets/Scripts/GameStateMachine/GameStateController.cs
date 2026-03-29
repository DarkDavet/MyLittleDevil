using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    private Dictionary<Type, GameState> _statesMap = new();
    private GameState StateCurrent { get; set; }

    public void AddState(GameState state)
    {
        _statesMap.Add(state.GetType(), state);
    }

    public void SetState<T>() where T : GameState
    {
        var type = typeof(T);

        if (StateCurrent != null && StateCurrent.GetType() == type)
        {
            return;
        }

        if (_statesMap.TryGetValue(type, out var newState))
        {
            StateCurrent?.Exit();
            StateCurrent = newState;
            StateCurrent.Enter();
        }
    }

    public void Update()
    {
        StateCurrent?.Update();
    }
}
