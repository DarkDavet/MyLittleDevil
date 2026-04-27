using UnityEngine;
using UnityEngine.UI;

public class NoFundsPanel : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private float autoCloseDelay = 2f;

    private void Awake()
    {
        gameObject.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(OnClose);
    }

    private void OnClose()
    {
        HidePanel();
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
        if (autoCloseDelay > 0)
        {
            Invoke(nameof(HidePanel), autoCloseDelay);
        }
    }

    private void HidePanel()
    {
        CancelInvoke();
        gameObject.SetActive(false);
    }
}