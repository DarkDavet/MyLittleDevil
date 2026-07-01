using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton manager that handles opening and closing managed UI windows.
/// Windows are registered via the inspector using UIWindow references with assigned WindowID values.
/// Only one window can be open at a time — opening a new window closes the current one.
/// </summary>
public class UIWindowsManager : MonoBehaviour
{
    public static UIWindowsManager Instance { get; private set; }

    [SerializeField] private List<UIWindow> windows;
    [SerializeField] private WindowID startWindow;

    private Dictionary<WindowID, UIWindow> windowDictionary = new Dictionary<WindowID, UIWindow>();
    private UIWindow currentWindow;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InitWindows();
    }

    private void Start()
    {
        if (startWindow != WindowID.None)
        {
            OpenWindow(startWindow);
        }
    }

    private void InitWindows()
    {
        foreach (var window in windows)
        {
            if (window == null) continue;

            if (!windowDictionary.ContainsKey(window.WindowID))
            {
                windowDictionary.Add(window.WindowID, window);
                window.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError($"Duplicate WindowID: {window.WindowID} on object {window.name}!");
            }
        }

        if (startWindow != WindowID.None && !windowDictionary.ContainsKey(startWindow))
        {
            Debug.LogWarning($"StartWindow ID '{startWindow}' was not found in the registered windows list. " +
                             $"Make sure a UIWindow with this WindowID exists in the inspector.");
        }
    }

    /// <summary>Opens the window with the given ID. Closes the currently open window first.</summary>
    public void OpenWindow(WindowID id)
    {
        if (windowDictionary.TryGetValue(id, out UIWindow window))
        {
            if (currentWindow != null)
            {
                currentWindow.Close();
            }

            currentWindow = window;
            currentWindow.Open();
        }
    }

    /// <summary>Convenience overload for use with Button.OnClick in the inspector (uses enum dropdown).</summary>
    public void OpenWindowFromButton(WindowID id)
    {
        OpenWindow(id);
    }

    /// <summary>Closes the currently open window.</summary>
    public void CloseCurrentWindow()
    {
        if (currentWindow != null)
        {
            currentWindow.Close();
            currentWindow = null;
        }
    }

    /// <summary>Returns true if the window with the given ID is currently open.</summary>
    public bool IsWindowOpen(WindowID id)
    {
        return currentWindow != null && currentWindow.WindowID == id;
    }
}
