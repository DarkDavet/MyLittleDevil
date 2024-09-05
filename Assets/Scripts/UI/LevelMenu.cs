using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;
    //public GameObject levelButtons;

    private void Awake()
    {
        //ButtonsToArray();
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        // Ensure the buttons array is not null and has elements
        if (buttons != null && buttons.Length > 0)
        {
            // Disable all buttons initially
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] != null) // Check if the button is not null
                {
                    buttons[i].interactable = false;
                }
                else
                {
                    Debug.LogError("Button at index " + i + " is not assigned in the inspector.");
                }
            }
            // Enable buttons up to the unlocked level
            for (int i = 0; i < unlockedLevel && i < buttons.Length; i++)
            {
                if (buttons[i] != null) // Check if the button is not null
                {
                    buttons[i].interactable = true;
                }
            }
        }
        else
        {
            Debug.LogError("Buttons array is not initialized or empty.");
        }
    }
    public void OpenLevel(int levelID)
    {
        string levelName = "Level" + levelID;
        SceneManager.LoadScene(levelName);
    }

    /*private void ButtonsToArray()
    {
        int childCount = levelButtons.transform.childCount;
        buttons = new Button[childCount];
        for (int i = 0; i < childCount;i++)
        {
            buttons[i] = levelButtons.transform.GetChild(i).gameObject.GetComponent<Button>();
        }
    }*/
}
