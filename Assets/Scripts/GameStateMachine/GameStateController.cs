using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController
{
    private Dictionary<Type, GameState> _statesMap = new();
    public static Action<Type> OnStateChangeRequest;
    private GameState StateCurrent { get; set; }

    public Player Player { get; set; }
    public CameraMoving CameraMoving { get; set; }
    public PlayerControls PlayerControls { get; set; }
    public IceShooting IceShooting { get; set; }
    public FireShooting FireShooting { get; set; }

    public GameStateController()
    {
        OnStateChangeRequest += SetStateByType;
    }

    public void AddState(GameState state) => _statesMap.Add(state.GetType(), state);
    
    private void SetStateByType(Type type)
    {
        if (StateCurrent != null && StateCurrent.GetType() == type) return;

        if (_statesMap.TryGetValue(type, out var newState))
        {
            StateCurrent?.Exit();
            StateCurrent = newState;
            StateCurrent.Enter();
            Debug.Log($"[State] Switched to: {type.Name}");
        }
    }

    public void SetState<T>() where T : GameState => SetStateByType(typeof(T));

    public void Update() => StateCurrent?.Update();

    public void Dispose() => OnStateChangeRequest -= SetStateByType;
}

