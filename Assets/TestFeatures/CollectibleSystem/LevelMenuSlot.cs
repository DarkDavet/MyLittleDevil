using CollectibleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenuSlot : MonoBehaviour
{
    [Header("Настройки уровня")]
    public string levelId; // Должен совпадать с тем, что в SaveKey

    [Header("Ссылки на UI")]
    public CollectibleStatWidget coinWidget;
    public CollectibleStatWidget crystalWidget;

    [Header("Ссылки на типы (для иконок)")]
    public CollectibleType coinType;
    public CollectibleType crystalType;

    private void Start()
    {
        DisplayLevelProgress();
    }

    public void DisplayLevelProgress()
    {
        string saveKey = "LevelStats_" + levelId;

        if (PlayerPrefs.HasKey(saveKey))
        {
            string json = PlayerPrefs.GetString(saveKey);
            // Используем структуру данных из твоего ScriptableObject
            var data = JsonUtility.FromJson<LevelCollectibleStats.LevelCollectibleStatsData>(json);

            foreach (var entry in data.collectibleStats)
            {
                if (entry.collectibleTypeId == coinType.Id)
                {
                    coinWidget.gameObject.SetActive(true);
                    coinWidget.Setup(coinType.Icon, entry.collectedCount, entry.totalCount);
                }
                else if (entry.collectibleTypeId == crystalType.Id)
                {
                    crystalWidget.gameObject.SetActive(true);
                    crystalWidget.Setup(crystalType.Icon, entry.collectedCount, entry.totalCount);
                }
            }
        }
        else
        {
            // Если сохранений нет — можно либо скрыть виджеты, либо поставить 0/Max
            coinWidget.gameObject.SetActive(false);
            crystalWidget.gameObject.SetActive(false);
        }
    }
}
