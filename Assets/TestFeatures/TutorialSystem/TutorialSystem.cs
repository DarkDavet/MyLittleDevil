using UnityEngine;
using UnityEngine.Events;

public class TutorialSystem : MonoBehaviour
{
    
    [SerializeField] private TutorialSetup _tutorialSetup;
    private int _pageMaxNum;
    private int _pageCurNum;
    
    public UnityEvent<TutorialSlot> OnOpenNewPage = new UnityEvent<TutorialSlot>();
    public UnityEvent OnTutorialFinished = new UnityEvent();
    public UnityEvent OnTutorialStarted = new UnityEvent();
    public UnityEvent OnLastPageOpened = new UnityEvent();
    

    
    public void StartTutorial()
    {
        // Check if tutorial has already been viewed
        if (_tutorialSetup == null)
        {
            Debug.LogError("TutorialSetup not initialized!");
            this.RequestPreviousState();
            return;
        }
        
        _tutorialSetup.LoadViewedStatus();
        
        if (_tutorialSetup.IsViewed)
        {
            Debug.Log("Tutorial already viewed, skipping");
            this.RequestPreviousState();
            return;
        }
        
        OnTutorialStarted.Invoke();
        
        _pageCurNum = 0;
        _pageMaxNum = _tutorialSetup.TutorialSlots.Count;
        
        if (_pageMaxNum > 0)
            OpenNewPage();
        else
            CloseTutorial();
    }
    
    public void OpenNewPage()
    {
        if (_tutorialSetup == null)
        {
            Debug.LogError("No tutorial setup found!");
            this.RequestPreviousState();
        }
        
        if (_pageCurNum < _pageMaxNum && _pageCurNum >= 0)
        {
            if (_pageCurNum == _pageMaxNum - 1)
            {
                OnOpenNewPage.Invoke(_tutorialSetup.TutorialSlots[_pageCurNum]);
                OnLastPageOpened.Invoke();
                Debug.Log("tut page: " + _pageCurNum + " / " + _pageMaxNum);
            }
            else
            {
                OnOpenNewPage.Invoke(_tutorialSetup.TutorialSlots[_pageCurNum]);
                Debug.Log("tut page: " + _pageCurNum + " / " + _pageMaxNum);
            }
            _pageCurNum++;
        }
        else
        {
            CloseTutorial();
        }
    }
    
    private void CloseTutorial()
    {
        _pageCurNum = 0;
        _pageMaxNum = 0;
        
        // Mark tutorial as viewed
        if (_tutorialSetup != null)
        {
            _tutorialSetup.MarkAsViewed();
            _tutorialSetup.SaveViewedStatus();
        }
        
        OnTutorialFinished.Invoke();
    }
}