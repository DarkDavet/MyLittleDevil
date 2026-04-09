using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialContainerUI : MonoBehaviour
{
    [SerializeField] private TutorialSystem _tutorialSystem;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _contentText;
    [SerializeField] private GameObject _nextButton;

    void Start()
    {
        _tutorialSystem.OnOpenNewPage.AddListener(OnNewPageOpened);
        _tutorialSystem.OnTutorialFinished.AddListener(OnTutorialFinished);
        _tutorialSystem.OnTutorialStarted.AddListener(OnTutorialStarted);
    }

    private void OnNewPageOpened(TutorialSlot tutorialSlot)
    {
        _titleText.text = tutorialSlot.title;
        _contentText.text = tutorialSlot.content;
        _nextButton.SetActive(tutorialSlot.showNextButton);
    }

    private void OnTutorialFinished()
    {
        gameObject.SetActive(false);
    }

    private void OnTutorialStarted()
    {
        gameObject.SetActive(true);
    }
}