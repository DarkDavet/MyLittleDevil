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
    public UnityEvent OnLastPageOpened = new UnityEvent();

    public void Init(TutorialStorage tutorialStorage)
    {
        _tutorialStorage = tutorialStorage;
    }

    public void StartTutorial(string tut_id)
    {
        if (_tutorialStorage == null)
        {
            Debug.LogError("TutorialStorage not initialized!");
            return;
        }

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
        if (_currentTutSetup == null)
        {
            Debug.LogError("No tutorial setup found!");
            return;
        }

        if (_pageCurNum < _pageMaxNum && _pageCurNum >= 0)
        {
            if (_pageCurNum == _pageMaxNum - 1)
            {
                OnOpenNewPage.Invoke(_currentTutSetup.TutorialSlots[_pageCurNum]);
                OnLastPageOpened.Invoke();
                Debug.Log("tut page: " + _pageCurNum + " / " + _pageMaxNum);
            }
            else
            {
                OnOpenNewPage.Invoke(_currentTutSetup.TutorialSlots[_pageCurNum]);
                Debug.Log("tut page: " + _pageCurNum + " / " + _pageMaxNum);
            }
            _pageCurNum++;
        }
        else
        {
            CloseTutorial();
        }
    }

    public void CloseTutorialNow()
    {
        CloseTutorial();
    }

    private bool FindRequiredTutorial(string tut_id)
    {
        if (_tutorialStorage == null || _tutorialStorage.TutorialSetups == null)
        {
            Debug.LogError("TutorialStorage or TutorialSetups is null!");
            return false;
        }

        foreach (var setup in _tutorialStorage.TutorialSetups)
        {
            if (setup != null && setup.Id == tut_id)
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
