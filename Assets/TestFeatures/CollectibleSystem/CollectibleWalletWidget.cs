using CollectibleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleWalletWidget : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private WalletSlotUI slotPrefab;
    [SerializeField] private Transform container;

    [SerializeField] private List<CollectibleType> allTypes;

    private List<GameObject> activeSlots = new List<GameObject>();

    private void OnEnable()
    {
        if (CollectibleManager.Instance != null)
        {
            CollectibleManager.Instance.OnBalanceChanged += RefreshUI;
        }
        RefreshUI();
    }
    private void OnDisable()
    {
        if (CollectibleManager.Instance != null)
        {
            CollectibleManager.Instance.OnBalanceChanged -= RefreshUI;
        }
    }

    public void RefreshUI()
    {
        foreach (var slot in activeSlots) Destroy(slot);
        activeSlots.Clear();

        if (CollectibleManager.Instance == null) return;

        foreach (var type in allTypes)
        {
            int count = CollectibleManager.Instance.GetItemCount(type.Id);

            if (count > 0)  // убрать условие, если нужны 0 в шлавном меню !!!
            {
                WalletSlotUI newSlot = Instantiate(slotPrefab, container);
                newSlot.Setup(type.Icon, count);
                activeSlots.Add(newSlot.gameObject);
            }
        }
    }
}
