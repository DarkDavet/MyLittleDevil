using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLG_EntryPoint : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private DialoguesStorage dlgStorage;
    [SerializeField] private DialogueSystem dlgSystem;
    [SerializeField] private DialogueContainerUI dlgUI;

    [SerializeField] private int nextSceneId;

    [Header("Dialogue ID:")]
    [SerializeField] private string dlg_Id;

    private void Awake()
    {
        GameEvents.OnDialogueFinished += OnDialogueFinished;
    }
    private void Start()
    {
        dlgSystem.Init(dlgStorage);
        dlgUI.Init();

        dlgSystem.StartDialogue(dlg_Id);
    }

    private void OnDialogueFinished()
    {
        sceneLoader.OpenLevel(nextSceneId);
    }

}
