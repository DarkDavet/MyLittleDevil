using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject victoryScreen;
    public GameObject inventoryScreen;

    private GameObject currentScreen;

    public void OpenInventoryFromGameOver()
    {
        currentScreen = gameOverScreen;
        gameOverScreen.SetActive(false);
        inventoryScreen.SetActive(true);
    }

    public void OpenInventoryFromVictory()
    {
        currentScreen = victoryScreen;
        victoryScreen.SetActive(false);
        inventoryScreen.SetActive(true);
    }

    public void CloseInventory()
    {
        inventoryScreen.SetActive(false);
        if (currentScreen != null)
        {
            currentScreen.SetActive(true);
        }
    }
}
