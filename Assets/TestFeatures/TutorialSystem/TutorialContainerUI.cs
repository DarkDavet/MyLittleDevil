using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialContainerUI : MonoBehaviour
{
    [SerializeField] private TutorialSystem _tutorialSystem;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _contentText;
    [SerializeField] private GameObject _nextButton;
    [SerializeField] private GameObject _closeButton;
    [SerializeField] private Image _backgroundImage;

    void Awake()
    {
        _tutorialSystem.OnOpenNewPage.AddListener(OnNewPageOpened);
        _tutorialSystem.OnTutorialFinished.AddListener(OnTutorialFinished);
        _tutorialSystem.OnTutorialStarted.AddListener(OnTutorialStarted);
        _tutorialSystem.OnLastPageOpened.AddListener(ShowCloseButton);
    }

    private void OnNewPageOpened(TutorialSlot tutorialSlot)
    {
        _titleText.text = tutorialSlot.title;
        _contentText.text = tutorialSlot.content;
        _nextButton.SetActive(tutorialSlot.showNextButton);
        _closeButton.SetActive(false);
        if (tutorialSlot.backgroundImage != null)
        {
            _backgroundImage.sprite = tutorialSlot.backgroundImage;
            _backgroundImage.enabled = true;
        }
        else
        {
            _backgroundImage.enabled = false;
        }
    }

    private void OnTutorialFinished()
    {
        gameObject.SetActive(false);
    }

    private void OnTutorialStarted()
    {
        gameObject.SetActive(true);
    }

    private void ShowCloseButton()
    {
        _nextButton.SetActive(false);
        _closeButton.SetActive(true);
    }
}