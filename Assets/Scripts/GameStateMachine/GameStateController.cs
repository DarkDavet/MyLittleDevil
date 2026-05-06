using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController
{
    private Dictionary<Type, GameState> _statesMap = new();
    public static Action<Type> OnStateChangeRequest;
    public static Action<Type> OnForceStateChangeRequest;
    public static Action OnBackRequest;

    public GameState StateCurrent { get; private set; }
    public GameState StatePrevious { get; private set; }
    public Player Player { get; set; }
    public InventoryObject Inventory { get; set; }
    public CameraMoving CameraMoving { get; set; }
    public PlayerInputHandler PlayerInput { get; set; }
    public IceShooting IceShooting { get; set; }
    public FireShooting FireShooting { get; set; }
    public UIManager UIManager { get; set; }
    public SceneLoader SceneLoader { get; set; }
    public TutorialSystem TutorialSystem { get; set; }
    public TimeReverseController TimeReverseController { get; set; }

    public GameStateController()
    {
        OnStateChangeRequest += SetStateByType;
        OnForceStateChangeRequest += ForceSetStateByType;
        OnBackRequest += BackToPreviousState;
    }

    public void AddState(GameState state) => _statesMap.Add(state.GetType(), state);

    private void SetStateByType(Type type)
    {
        if (StateCurrent != null && StateCurrent.IsFinalState) return;

        SwitchStateInternal(type);
    }

    private void ForceSetStateByType(Type type)
    {
        Debug.Log($"[State] Force switching to {type.Name}...");
        SwitchStateInternal(type);
    }

    private void SwitchStateInternal(Type type)
    {
        if (StateCurrent != null && StateCurrent.GetType() == type) return;

        if (_statesMap.TryGetValue(type, out var newState))
        {
            StateCurrent?.Exit();
            StatePrevious = StateCurrent;
            StateCurrent = newState;
            StateCurrent.Enter();
        }
    }

    public void BackToPreviousState()
    {
        if (StatePrevious != null)
        {
            SetStateByType(StatePrevious.GetType());
        }
    }

    public void SetState<T>() where T : GameState => SetStateByType(typeof(T));

    public void Update() => StateCurrent?.Update();

    public void Dispose()
    {
        OnStateChangeRequest -= SetStateByType;
        OnForceStateChangeRequest -= ForceSetStateByType;
        OnBackRequest -= BackToPreviousState;
    }   
}

