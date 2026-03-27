using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DialogueSystem : MonoBehaviour
{
    private DialoguesStorage _dialoguesStorage;
    private DialogueSetup _currentDlgSetup;
    private int _lineMaxNum;
    private int _lineCurNum;

    public UnityEvent<DialogueSlot> OnSetNewLine = new UnityEvent<DialogueSlot>();
    public UnityEvent OnDialogueFinished = new UnityEvent();
    public UnityEvent OnDialogueStarted = new UnityEvent();

    public void Init(DialoguesStorage dialoguesStorage)
    {
        _dialoguesStorage = dialoguesStorage;
    }

    public void StartDialogue(string dlg_id)
    {
        OnDialogueStarted.Invoke();
        if (FindRequiredDialogue(dlg_id))
        {
            _lineCurNum = 0;
            _lineMaxNum = _currentDlgSetup.DialogueSlots.Count;

            if (_lineMaxNum > 0)
                SetNewLine();
            else
                FinishDialogue();
        }
        else
        {
            Debug.LogError($"Dialogue with ID {dlg_id} not found in storage!");
        }
    }

    public void SetNewLine()
    {
        if (_currentDlgSetup == null) return;

        if (_lineCurNum < _lineMaxNum)
        {
            OnSetNewLine.Invoke(_currentDlgSetup.DialogueSlots[_lineCurNum]);
            Debug.Log($"dlg line: {_lineCurNum} / {_lineMaxNum}");
            _lineCurNum++;
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
        _lineCurNum = 0;
        _lineMaxNum = 0;
        OnDialogueFinished.Invoke();
    }
}
