using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLG_EntryPoint : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private DialoguesStorage dlgStorage;
    [SerializeField] private DialogueSystem dlgSystem;
    [SerializeField] private DialogueContainerUI dlgUI;

    [Header("Dialogue ID:")]
    [SerializeField] private string dlg_Id;

    [Header("Music")]
    [SerializeField] private string musicTrackId;

    private void Awake()
    {
        GameEvents.OnDialogueFinished += OnDialogueFinished;
    }
    private void Start()
    {
        dlgSystem.Init(dlgStorage);
        dlgUI.Init();

        if (!string.IsNullOrEmpty(musicTrackId))
        {
            AudioManager.instance.PlayMusic(musicTrackId);
        }

        dlgSystem.StartDialogue(dlg_Id);
    }

    private void OnDialogueFinished()
    {
        sceneLoader.UnlockNewScene();
        sceneLoader.OpenNextScene();
    }

}
