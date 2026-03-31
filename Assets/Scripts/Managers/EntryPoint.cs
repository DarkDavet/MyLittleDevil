using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
   // [SerializeField] private DialoguesStorage dlgStorage;
   // [SerializeField] private DialogueSystem dlgSystem;
   // [SerializeField] private DialogueContainerUI dlgUI;
    [SerializeField] private GameStateContext gameStateManager;

    private void Start()
    {
        /*dlgSystem.Init(dlgStorage);
        dlgUI.Init();
        dlgSystem.StartDialogue("dlg_1");*/
        gameStateManager.Init();
    }
}
