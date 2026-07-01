using System.Collections.Generic;
using AchievementSystem;
using AchievementSystem.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Achievements window — manages slots, counts, and live event updates.
/// Extends UIWindow for animation lifecycle, implements IAchievementListener for live updates.
/// </summary>
public class AchievementsWindow : UIWindow, IAchievementListener
{
    [Header("Content")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotsContainer;
    [SerializeField] private TextMeshProUGUI totalCountText;
    [SerializeField] private TextMeshProUGUI unlockedCountText;

    private List<AchievementSlotUI> slotUIs = new List<AchievementSlotUI>();

    protected override void Awake()
    {
        base.Awake();
        InitializeSlots();
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        AchievementSystemCore.Instance?.AchievementManager?.AddListener(this);
        UpdateCounts();
        RefreshExistingSlots();
    }

    protected override void OnClose()
    {
        base.OnClose();
        AchievementSystemCore.Instance?.AchievementManager?.RemoveListener(this);
    }

    private void InitializeSlots()
    {
        if (AchievementSystemCore.Instance == null || AchievementSystemCore.Instance.AchievementManager == null)
            return;

        var types = AchievementSystemCore.Instance.GetAllAchievements();
        var progressDict = AchievementSystemCore.Instance.AchievementManager.GetAchievementProgress();

        foreach (Transform child in slotsContainer) Destroy(child.gameObject);
        slotUIs.Clear();

        foreach (var type in types)
        {
            if (progressDict.TryGetValue(type.Id, out var data))
            {
                GameObject slotObj = Instantiate(slotPrefab, slotsContainer);
                AchievementSlotUI slotUI = slotObj.GetComponent<AchievementSlotUI>();
                if (slotUI != null)
                {
                    slotUI.Setup(type, data);
                    slotUIs.Add(slotUI);
                }
            }
        }
    }

    private void RefreshExistingSlots()
    {
        if (AchievementSystemCore.Instance == null) return;

        var manager = AchievementSystemCore.Instance.AchievementManager;
        var progressDict = manager.GetAchievementProgress();
        var typesDict = manager.GetAchievementTypes();

        foreach (var slot in slotUIs)
        {
            if (progressDict.TryGetValue(slot.AchievementId, out var data))
            {
                slot.Setup(typesDict[slot.AchievementId], data);
            }
        }
    }

    public void UpdateCounts()
    {
        var manager = AchievementSystemCore.Instance?.AchievementManager;
        if (manager == null) return;

        if (totalCountText != null)
            totalCountText.text = manager.GetTotalAchievementCount().ToString();

        if (unlockedCountText != null)
            unlockedCountText.text = manager.GetTotalUnlockedCount().ToString();
    }

    // IAchievementListener callbacks
    public void OnAchievementUnlocked(AchievementType achievementType)
    {
        UpdateCounts();
        UpdateSpecificSlot(achievementType.Id);
    }

    public void OnAchievementProgress(AchievementType achievementType, int currentProgress, int requiredProgress)
    {
        UpdateCounts();
        UpdateSpecificSlot(achievementType.Id);
    }

    private void UpdateSpecificSlot(string id)
    {
        foreach (var slot in slotUIs)
        {
            if (slot.AchievementId == id)
            {
                var manager = AchievementSystemCore.Instance.AchievementManager;
                if (manager == null) return;
                slot.Setup(manager.GetAchievementTypes()[id], manager.GetAchievementProgress()[id]);
                break;
            }
        }
    }
}
