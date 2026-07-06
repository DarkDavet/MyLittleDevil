using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public SceneDatabase sceneDatabase;
    public LevelButton[] levelButtons;

    public SceneData firstScene;

    private void Awake()
    {
        RefreshMenu();
      //  PlayerPrefs.DeleteAll(); //Delete after testing
    }

    //placeholder of future entry point of main menu
    private void Start()
    {
        AudioManager.instance.StopMusic();
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

    public void ContinueGame()
    {
        string lastID = PlayerPrefs.GetString("LastPlayedLevel");

        foreach (SceneData level in sceneDatabase.sceneDatabase)
        {
            if (level.sceneID == lastID)
            {
                SceneManager.LoadScene(level.sceneName);
                return;
            }
        }
        if (firstScene != null)
        {
            PlayerPrefs.SetString("LastPlayedLevel", firstScene.sceneID);
            PlayerPrefs.Save();

            SceneManager.LoadScene(firstScene.sceneName);
        }

        Debug.LogWarning("There is not this scene: " + lastID);
    }

    [ContextMenu("Reset Progress")]
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        RefreshMenu();
    }
}
