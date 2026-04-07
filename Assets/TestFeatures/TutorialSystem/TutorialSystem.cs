using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialSystem : MonoBehaviour
{
    private int _pageMaxNum;
    private int _pageCurNum;

    public UnityEvent<DialogueSlot> OnOpenNewPage = new UnityEvent<DialogueSlot>();
    public UnityEvent OnTutorialFinished = new UnityEvent();
    public UnityEvent OnTutorialStarted = new UnityEvent();

    public void Init()
    {
        ;
    }

    public void StartDialogue(string tut_id)
    {
        OnTutorialStarted.Invoke();
        if (FindRequiredTutorial(tut_id))
        {
            _pageCurNum = 0;

            if (_pageMaxNum > 0)
                OpenNewPage();
            else
                CloseTutorial();
        }
        else
        {
            Debug.LogError($"Tutorial with ID {tut_id} not found in storage!");
        }
    }

    public void OpenNewPage()
    {
        
    }

    private bool FindRequiredTutorial(string tut_id)
    {
        return false;
    }

    private void CloseTutorial()
    {
        _pageCurNum = 0;
        _pageMaxNum = 0;
        OnTutorialFinished.Invoke();
    }
}
