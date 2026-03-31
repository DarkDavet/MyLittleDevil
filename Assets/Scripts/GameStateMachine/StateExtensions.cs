using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateExtensions
{
    public static void RequestState<T>(this object sender) where T : GameState
    {
        GameStateController.OnStateChangeRequest?.Invoke(typeof(T));
    }

    public static void RequestPreviousState(this object sender)
    {
        GameStateController.OnBackRequest?.Invoke();
    }
}
