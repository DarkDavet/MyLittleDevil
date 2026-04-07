using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneData nextScene;
    public void OpenNextScene()
    {
        if (nextScene == null) return;

        PlayerPrefs.SetString("LastPlayedLevel", nextScene.sceneID);
        PlayerPrefs.Save();
        SceneManager.LoadScene(nextScene.sceneName);
    }

    public void UnlockNewScene()
    {
        if (nextScene == null) return;

        PlayerPrefs.SetInt("Unlocked_" + nextScene.sceneID, 1);
        PlayerPrefs.Save();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
