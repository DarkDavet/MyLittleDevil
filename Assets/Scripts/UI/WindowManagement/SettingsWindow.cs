using UnityEngine;

/// <summary>
/// Settings window.
/// Place custom logic here (e.g. applying volume sliders, saving control bindings).
/// </summary>
public class SettingsWindow : UIWindow
{
    protected override void OnOpen()
    {
        base.OnOpen();
        // TODO: load current settings values into UI controls
    }

    protected override void OnClose()
    {
        base.OnClose();
        // TODO: save control bindings, persist volume changes
    }
}
