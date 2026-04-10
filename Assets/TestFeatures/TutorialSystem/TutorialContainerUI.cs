using DG.Tweening;
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

    private CanvasGroup _canvasGroup;

    private Tween _contentTween;
    private float _animDuration = 0.25f;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
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
        
        _contentTween?.Kill();
        _contentText.alpha = 0;
        _contentText.DOFade(1, _animDuration).SetUpdate(true);
    }

    private void OnTutorialFinished()
    {
        _canvasGroup.DOFade(0, _animDuration).SetUpdate(true).OnComplete(() =>
        {
            CleanListeners();
            this.RequestPreviousState();
        });
    }

    private void OnTutorialStarted()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, _animDuration).SetUpdate(true);
        _canvasGroup.blocksRaycasts = true;
    }

    private void ShowCloseButton()
    {
        _nextButton.SetActive(false);
        _closeButton.SetActive(true);
    }
    private void CleanListeners()
    {
        if (_tutorialSystem != null)
        {
            _tutorialSystem.OnOpenNewPage.RemoveListener(OnNewPageOpened);
            _tutorialSystem.OnTutorialFinished.RemoveListener(OnTutorialFinished);
            _tutorialSystem.OnTutorialStarted.RemoveListener(OnTutorialStarted);
            _tutorialSystem.OnLastPageOpened.RemoveListener(ShowCloseButton);
        }
    }

    private void OnDestroy()
    {
        CleanListeners();
    }
}