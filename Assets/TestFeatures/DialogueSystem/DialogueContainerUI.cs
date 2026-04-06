using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueContainerUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private DialogueSlotUI left_slot;
    [SerializeField] private DialogueSlotUI right_slot;

    private CanvasGroup _canvasGroup;
    private Tween _textTween;
    private DialogueSystem _dialogueSystem;
    private DialogueSlot _dlgSlot;

    float anim_duration = 0.25f;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _dialogueSystem = GetComponent<DialogueSystem>();
    }
    public void Init()
    {
        _canvasGroup.alpha = 0;
        _dialogueSystem.OnSetNewLine.AddListener(UpdateSlot);
        _dialogueSystem.OnDialogueFinished.AddListener(Close);
        _dialogueSystem.OnDialogueStarted.AddListener(ShowWindow);
    }

    private void ShowWindow()
    {
        _canvasGroup.DOFade(1, anim_duration);
        _canvasGroup.blocksRaycasts = true;
    }

    private void UpdateSlot(DialogueSlot dlgSlot)
    {
        _dlgSlot = dlgSlot;
        _textTween?.Kill();
        
        if (_dlgSlot.visibility == SlotOwnerVisibilityType.Left)
        {
            Debug.Log("Left");
            left_slot.icon.sprite = _dlgSlot.icon;
            left_slot.title.text = _dlgSlot.title;
            left_slot.title.color = _dlgSlot.title_color;

            left_slot.canvasGroup.DOFade(1, anim_duration);
            right_slot.canvasGroup.DOFade(0, anim_duration);
        }
        else if (_dlgSlot.visibility == SlotOwnerVisibilityType.Right)
        {
            Debug.Log("Right");
            right_slot.icon.sprite = _dlgSlot.icon;
            right_slot.title.text = _dlgSlot.title;
            left_slot.title.color = _dlgSlot.title_color;

            left_slot.canvasGroup.DOFade(0, anim_duration);
            right_slot.canvasGroup.DOFade(1, anim_duration);
        }
        else if (_dlgSlot.visibility == SlotOwnerVisibilityType.None)
        {
            Debug.Log("None");
            left_slot.canvasGroup.DOFade(0, anim_duration);
            right_slot.canvasGroup.DOFade(0, anim_duration);
        }

        text.text = "";
        float textSpeed = 0.05f;
        float duration = dlgSlot.string_text.Length * textSpeed;


        _textTween = DOTween.To(() => text.text, x => text.text = x, _dlgSlot.string_text, duration).SetEase(Ease.Linear);
    }

    private void Close()
    {
        _canvasGroup.DOFade(0, anim_duration).OnComplete(() => {
            _canvasGroup.blocksRaycasts = false;
            GameEvents.TriggerDialogueFinished();
        });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_textTween != null && _textTween.IsActive() && !_textTween.IsComplete())
        {
            _textTween.Complete();
            return; 
        }

        _dialogueSystem.SetNewLine();
    }
}
