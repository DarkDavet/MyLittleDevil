using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public LevelButton[] levelButtons;

    private void Awake()
    {
        RefreshMenu();
    }

    public void RefreshMenu()
    {
        foreach (var btn in levelButtons)
        {
            if (btn != null)
            {
                btn.UpdateState();
            }
        }
    }

    [ContextMenu("Reset Progress")]
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        RefreshMenu();
    }
}
