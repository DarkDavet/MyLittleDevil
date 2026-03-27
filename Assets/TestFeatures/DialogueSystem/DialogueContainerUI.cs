using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueContainerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private DialogueSlotUI left_slot;
    [SerializeField] private DialogueSlotUI right_slot;

    private DialogueSystem _dialogueSystem;
    private DialogueSlot _dlgSlot;

    private void Awake()
    {
        _dialogueSystem = GetComponent<DialogueSystem>();
    }
    public void Init()
    {
        _dialogueSystem.OnSetNewLine.AddListener(UpdateSlot);
        _dialogueSystem.OnDialogueFinished.AddListener(Close);
    }

    private void UpdateSlot(DialogueSlot dlgSlot)
    {
        _dlgSlot = dlgSlot;
        if (_dlgSlot.visibility == SlotOwnerVisibilityType.Left)
        {
            left_slot.gameObject.SetActive(true);
            right_slot.gameObject.SetActive(false);
            left_slot.icon.sprite = _dlgSlot.icon;
            left_slot.title.text = _dlgSlot.title;
        }
        if (_dlgSlot.visibility == SlotOwnerVisibilityType.Right)
        {
            left_slot.gameObject.SetActive(true);
            right_slot.gameObject.SetActive(false);
            right_slot.icon.sprite = _dlgSlot.icon;
            right_slot.title.text = _dlgSlot.title;
        }
    }

    private void Close()
    {
        
    }
}
