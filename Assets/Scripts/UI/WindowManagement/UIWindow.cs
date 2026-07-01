using DG.Tweening;
using UnityEngine;

/// <summary>
/// Base class for all managed UI windows.
/// Provides fade + scale animations via DOTween and a WindowID for registration.
/// Subclasses can override OnOpen() and OnClose() for custom logic.
/// </summary>
public abstract class UIWindow : MonoBehaviour
{
    [SerializeField] private WindowID windowID;
    public WindowID WindowID => windowID;

    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private Vector3 openScale = Vector3.one;
    [SerializeField] private Vector3 closeScale = new Vector3(0.8f, 0.8f, 0.8f);

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        rectTransform.localScale = closeScale;

        // Animation: fade in and scale up
        canvasGroup.DOFade(1f, fadeDuration).SetUpdate(true);
        rectTransform.DOScale(openScale, fadeDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true); // SetUpdate(true) allows animations to run when Time.timeScale == 0

        OnOpen();
    }

    public virtual void Close()
    {
        canvasGroup.DOFade(0f, fadeDuration).SetUpdate(true);
        rectTransform.DOScale(closeScale, fadeDuration)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                OnClose();
            });
    }

    /// <summary>Called after the window becomes visible (after animation starts).</summary>
    protected virtual void OnOpen() { }

    /// <summary>Called after the window finishes closing and is deactivated.</summary>
    protected virtual void OnClose() { }
}
