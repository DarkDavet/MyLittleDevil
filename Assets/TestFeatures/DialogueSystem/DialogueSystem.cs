using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DialogueSystem : MonoBehaviour
{
    private DialoguesStorage _dialoguesStorage;
    private DialogueSetup _currentDlgSetup;
    private int line_max_num;
    private int line_cur_num;

    public UnityEvent<DialogueSlot> OnSetNewLine = new UnityEvent<DialogueSlot>();
    public UnityEvent OnDialogueFinished = new UnityEvent();
    public void Init(DialoguesStorage dialoguesStorage)
    {
        _dialoguesStorage = dialoguesStorage;
    }

    public void StartDialogue(string dlg_id)
    {
        line_cur_num = 0;
        if (FindRequiredDialogue(dlg_id))
        {
            line_max_num = _currentDlgSetup.DialogueSlots.Count;
            SetNewLine(); 
        }
    }

    public void SetNewLine()
    {
        if (line_cur_num < line_max_num)
        {
           OnSetNewLine.Invoke(_currentDlgSetup.DialogueSlots[line_cur_num]);
           line_cur_num++;
        }
        else
        {
            FinishDialogue();
        }
    }

    private bool FindRequiredDialogue(string dlg_id)
    {
        foreach (var setup in _dialoguesStorage.DialogueSetups)
        {
            if (setup.Id == dlg_id)
            {
                _currentDlgSetup = setup;
                return true;
            }
        }
        return false;
    }

    private void FinishDialogue()
    {
        _currentDlgSetup = null;
        line_cur_num = 0;
        line_max_num = 0;
        OnDialogueFinished.Invoke();
    }
}
