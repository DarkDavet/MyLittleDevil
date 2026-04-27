using CollectibleSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationManager : MonoBehaviour
{
    [SerializeField] private CharacterPreview previewer;

    [Header("UI Panels")]
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private GameObject noFundsPanel;

    private CustomizationItem currentSelectedItem;

    public void SelectItem(CustomizationItem item)
    {
        currentSelectedItem = item;
        previewer.ApplyPreview(item);
    }

    public void OnBuyButtonClick()
    {
        if (currentSelectedItem == null) return;
        if (currentSelectedItem.IsUnlocked) return; 

        int playerBalance = CollectibleManager.Instance.GetItemCount(currentSelectedItem.currencyType.Id);

        if (playerBalance >= currentSelectedItem.price)
        {
            confirmPanel.SetActive(true);
        }
        else
        {
            noFundsPanel.SetActive(true);
        }
    }

    public void ConfirmPurchase()
    {
        if (currentSelectedItem == null) return;

        string currencyId = currentSelectedItem.currencyType.Id;
        int price = currentSelectedItem.price;

        if (CollectibleManager.Instance.SpendItem(currencyId, price))
        {
            currentSelectedItem.Unlock();
            confirmPanel.SetActive(false);


            Debug.Log($"Purchased item: {currentSelectedItem.displayName}. cost: {price} {currencyId}");
        }
        else
        {
            confirmPanel.SetActive(false);
            noFundsPanel.SetActive(true);
        }
    }
}