using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public static LogicScript _instance;
    public InventoryObject inventory;
   
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private GameObject VictoryScreen;
    [SerializeField] private GameObject PlayerHealthBar;

    private void Awake()
    {
        _instance = this;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        GameOverScreen.SetActive(true);
        VictoryScreen.SetActive(false);
        inventory.Clear();
        //Destroy(PlayerHealthBar);

    }

    public void Victory()
    {
        VictoryScreen.SetActive(true);
        GameOverScreen.SetActive(false);
        inventory.Clear();
    }


}
