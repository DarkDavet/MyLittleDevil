using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISettingsSubsystem
{
    void Initialize();                // Загрузка настроек при старте игры
    void CacheCurrentState();         // Запоминаем состояние при ОТКРЫТИИ меню настроек
    void ApplyAndSave();              // Кнопка "Применить"
    void DiscardChanges();            // Кнопка "Отмена" (откат к кэшу)
    void ResetToDefault();            // Кнопка "Сбросить по умолчанию"
    bool HasUnsavedChanges();         // Проверка на наличие изменений
}
