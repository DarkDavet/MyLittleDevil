using CollectibleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSummaryUI : MonoBehaviour
{
    [SerializeField] private Transform container; // Сюда будут падать виджеты
    [SerializeField] private CollectibleStatWidget widgetPrefab;

    [Header("Настройки типов предметов")]
    [SerializeField] private CollectibleType coinType;
    [SerializeField] private CollectibleType crystalType;

    private void OnEnable()
    {
        ShowSummary();
    }
    public void ShowSummary()
    {
        // 1. Очищаем старые иконки, если они были
        foreach (Transform child in container) Destroy(child.gameObject);

        // 2. Берем статистику из менеджера
        var stats = LevelStatsManager.Instance.GetCurrentStats();
        if (stats == null) return;

        // 3. Проходим по всем данным, которые накопились за уровень
        foreach (var runtime in stats.runtimeStats)
        {
            // Определяем, какую иконку рисовать
            Sprite iconToDisplay = null;

            if (runtime.collectibleTypeId == coinType.Id)
                iconToDisplay = coinType.Icon;
            else if (runtime.collectibleTypeId == crystalType.Id)
                iconToDisplay = crystalType.Icon;

            // 4. Спавним виджет и передаем в него данные
            if (iconToDisplay != null)
            {
                var widget = Instantiate(widgetPrefab, container);
                int added = LevelStatsManager.Instance.GetAddedThisSession(runtime.collectibleTypeId);
                widget.Setup(iconToDisplay, runtime.collectedCount, runtime.totalCount, added);
            }
        }
    }
}
