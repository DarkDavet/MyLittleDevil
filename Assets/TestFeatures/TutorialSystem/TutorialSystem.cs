using UnityEngine;
using UnityEngine.Events;

public class TutorialSystem : MonoBehaviour
{
    private TutorialStorage _tutorialStorage;
    private TutorialSetup _currentTutSetup;
    private int _pageMaxNum;
    private int _pageCurNum;

    public UnityEvent<TutorialSlot> OnOpenNewPage = new UnityEvent<TutorialSlot>();
    public UnityEvent OnTutorialFinished = new UnityEvent();
    public UnityEvent OnTutorialStarted = new UnityEvent();

    public void Init(TutorialStorage tutorialStorage)
    {
        _tutorialStorage = tutorialStorage;
    }

    public void StartTutorial(string tut_id)
    {
        OnTutorialStarted.Invoke();
        if (FindRequiredTutorial(tut_id))
        {
            _pageCurNum = 0;
            _pageMaxNum = _currentTutSetup.TutorialSlots.Count;

            if (_pageMaxNum > 0)
                OpenNewPage();
            else
                CloseTutorial();
        }
        else
        {
            Debug.LogError("Tutorial with ID " + tut_id + " not found in storage!");
        }
    }

    public void OpenNewPage()
    {
        if (_currentTutSetup == null) return;

        if (_pageCurNum < _pageMaxNum)
        {
            OnOpenNewPage.Invoke(_currentTutSetup.TutorialSlots[_pageCurNum]);
            Debug.Log("tut page: " + _pageCurNum + " / " + _pageMaxNum);
            _pageCurNum++;
        }
        else
        {
            CloseTutorial();
        }
    }

    private bool FindRequiredTutorial(string tut_id)
    {
        foreach (var setup in _tutorialStorage.TutorialSetups)
        {
            if (setup.Id == tut_id)
            {
                _currentTutSetup = setup;
                return true;
            }
        }
        return false;
    }

    private void CloseTutorial()
    {
        _currentTutSetup = null;
        _pageCurNum = 0;
        _pageMaxNum = 0;
        OnTutorialFinished.Invoke();
    }
}
