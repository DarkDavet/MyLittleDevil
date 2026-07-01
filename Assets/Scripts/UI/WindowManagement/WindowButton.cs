using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach this to a Button GameObject to open a specific window when clicked.
/// Assign a UIWindow prefab in the inspector and the button will call OpenWindow
/// with that window's WindowID automatically.
/// </summary>
[RequireComponent(typeof(Button))]
public class WindowButton : MonoBehaviour
{
    [SerializeField] private WindowID windowID;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (button != null)
            button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        if (button != null)
            button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        if (windowID!= WindowID.None)
        {
            UIWindowsManager.Instance.OpenWindow(windowID);
        }     
    }
}
