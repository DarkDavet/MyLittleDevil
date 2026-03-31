using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents
{
    public static event Action OnLoose;
    public static event Action OnWin;
    public static event Action OnFightStarted;
    public static event Action OnFightFinished;
    public static event Action<int> OnDamageGot;  //is not used for a while
    public static event Action<int> OnHealthHealed;
    public static event Action<int> OnUpdatePlayerHealth;

    public static event Action<string> OnDialogueStarted;
    public static event Action OnDialogueFinished;


    public static void TriggerLoose() => OnLoose?.Invoke();
    public static void TriggerWin() => OnWin?.Invoke();
    public static void TriggerFightStarted() => OnFightStarted?.Invoke();
    public static void TriggerFightFinished() => OnFightFinished?.Invoke();
    public static void TriggerDialogueStarted(string id) => OnDialogueStarted?.Invoke(id);
    public static void TriggerDialogueFinished() => OnDialogueFinished?.Invoke();
    public static void TriggerUpdatedPlayerHealth(int health) => OnUpdatePlayerHealth?.Invoke(health);
    public static void TriggerHealthHealed(int health) => OnHealthHealed?.Invoke(health);
}
