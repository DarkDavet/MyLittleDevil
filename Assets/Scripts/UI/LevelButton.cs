using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private SceneData sceneData; 
    [SerializeField] private Button button;
    [SerializeField] private Image icon;

    public void UpdateState()
    {
        if (sceneData == null) return;

        bool isUnlocked = sceneData.isUnlockedByDefault ||
                          PlayerPrefs.GetInt("Unlocked_" + sceneData.sceneID, 0) == 1;

        button.interactable = isUnlocked;

        icon.color = isUnlocked ? Color.white : Color.gray;
    }

    public void Click()
    {
        SceneManager.LoadScene(sceneData.sceneName);
    }
}
