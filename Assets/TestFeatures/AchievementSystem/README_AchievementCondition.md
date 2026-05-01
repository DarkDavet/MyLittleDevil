# AchievementSystem — Полное руководство по использованию

## Обзор системы

AchievementSystem — это событийно-ориентированная система достижений для Unity 2D игры. Она позволяет легко добавлять достижения в игру, просто перетаскивая AchievementType ScriptableObject в Inspector.

## Архитектура системы

```
┌─────────────────────────────────────────────────────────────┐
│ AchievementSystemCore (ЕДИНСТВЕННЫЙ Singleton)              │
│ - DontDestroyOnLoad                                         │
│ - Все AchievementType назначены в Inspector                 │
│ - AchievementNotificationUI prefab (опционально)             │
│ - Persistent Notification Canvas (авто-создаётся)           │
│ - Единственная точка входа для всей системы                 │
│                                                             │
│ Использование:                                              │
│ AchievementSystemCore.Instance.UpdateProgress(...)          │
│ AchievementSystemCore.Instance.UnlockAchievement(...)       │
│ AchievementSystemCore.Instance.IsAchievementUnlocked(...)   │
└─────────────────────────────────────────────────────────────┘
                               │
                               │ хранит ссылку на
                               │
┌─────────────────────────────────────────────────────────────┐
│ AchievementManager (Plain MonoBehaviour, НЕ Singleton)      │
│ - Доступ: AchievementSystemCore.Instance.AchievementManager │
│ - RegisterAchievementTypes()                                │
│ - UpdateProgress()                                          │
│ - UnlockAchievement()                                       │
│ - Save/Load PlayerPrefs                                     │
│ - События: OnAchievementUnlocked, OnAchievementProgress     │
└─────────────────────────────────────────────────────────────┘
                               │
                               │ AchievementEvents (static bus)
                               │
┌─────────────────────────────────────────────────────────────┐
│ Persistent Notification Canvas (DontDestroyOnLoad)          │
│ - RenderMode: ScreenSpaceCamera                             │
│ - CanvasScaler: ScaleWithScreenSize                         │
│ - Уведомления создаются здесь (всегда на экране)            │
└─────────────────────────────────────────────────────────────┘
                               │
                               │ Сцены игры (не зависят от Canvas)
                               │
┌─────────────────────────────────────────────────────────────┐
│ Сцены игры                                                   │
│ ┌─────────────────────────────────────────────────────────┐ │
│ │ AchievementEventListener (на каждой сцене)              │ │
│ │ - onKillAchievements: [kill_10_enemies]                 │ │
│ │ - onCollectAchievements: [collect_20_coins]             │ │
│ └─────────────────────────────────────────────────────────┘ │
│ ┌─────────────────────────────────────────────────────────┐ │
│ │ AchievementPanelUI (только если нужен список)           │ │
│ └─────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
```

### Ключевые принципы архитектуры

1. **Один синглтон**: Только `AchievementSystemCore` является синглтоном с `DontDestroyOnLoad`
2. **Plain Component**: `AchievementManager` — обычный компонент, доступ к нему через `AchievementSystemCore.Instance.AchievementManager`
3. **Удобные методы-псевдонимы**: `AchievementSystemCore` предоставляет прямые методы для большинства операций:
   - `UpdateProgress(id, amount)` — обновить прогресс
   - `UnlockAchievement(id)` — разблокировать ачивку
   - `IsAchievementUnlocked(id)` — проверить статус
   - `GetAchievementProgress(id)` — получить текущий прогресс
   - `GetTotalUnlockedCount()` — получить количество разблокированных
   - `GetTotalAchievementCount()` — получить общее количество
   - `SaveProgress()` — сохранить прогресс
   - `LoadProgress()` — загрузить прогресс
   - `ResetProgress()` — сбросить прогресс

## Быстрый старт

### Шаг 1: Создайте AchievementType ScriptableObject

Right-click в Project → **Create** → **Achievement System** → **Achievement**

Настройте:
- **ID**: `kill_10_enemies` (уникальный идентификатор)
- **Title**: `Monster Slayer`
- **Description**: `Kill 10 enemies`
- **Required Progress**: `10`
- **Progress Type**: `Counter`
- **Category**: `Combat`
- **Icon**: Sprite для иконки ачивки

### Шаг 2: Создайте AchievementSystemCore

1. Создайте пустой GameObject → `AchievementSystemCore`
2. Добавьте компонент **`AchievementSystemCore`**
3. Перетащите ВСЕ AchievementType ScriptableObject из Project в поле **All Achievements**
4. (Опционально) Назначьте AchievementNotificationUI prefab в поле **Notification Prefab**
5. Persistent Notification Canvas создаётся автоматически (настраивается в Canvas Settings)

```
┌─────────────────────────────────────────────────┐
│ Achievement System Core                         │
├─────────────────────────────────────────────────┤
│ All Achievements:                               │
│   [0] → kill_10_enemies (AchievementType)       │
│   [1] → kill_50_enemies (AchievementType)       │
│   [2] → collect_20_coins (AchievementType)      │
│   ...                                           │
│   Size: N                                       │
├─────────────────────────────────────────────────┤
│ Notification Prefab:                            │
│   [None] (AchievementNotificationUI prefab)     │
├─────────────────────────────────────────────────┤
│ Canvas Settings:                                │
│   Auto Create Persistent Canvas: true           │
│   Canvas Resolution: (1920, 1080)               │
│   Canvas Match Width/Height: true               │
└─────────────────────────────────────────────────┘
```

#### Persistent Notification Canvas

При первом запуске система автоматически создаёт отдельный Canvas для уведомлений с `DontDestroyOnLoad`. Это гарантирует, что уведомления **всегда будут видны на экране**, независимо от того, в какой сцене находится игрок.

**Преимущества:**
- Уведомления не зависят от сцен — всегда поверх всего UI
- Автоматическая адаптация к любому разрешению экрана
- Не требует ручной настройки Canvas в каждой сцене
- Нет проблемы "исчезающего" `notificationParent` при переходах между сценами

### Шаг 3: Добавьте AchievementEventListener на сцену

1. Добавьте **AchievementEventListener** на любой GameObject в сцене
2. В Inspector перетащите AchievementType в соответствующие поля:

| Section | Описание |
|---|---|
| **On Kill Achievements** | Ачивки, которые обновляются при убийстве врага |
| **On Collect Achievements** | Ачивки, которые обновляются при подборе предмета |
| **On Damage Dealt Achievements** | Ачивки, которые обновляются при нанесении урона |
| **On Healed Achievements** | Ачивки, которые обновляются при лечении |
| **On Distance Traveled Achievements** | Ачивки, которые обновляются при прохождении дистанции |
| **On Dialogue Finished Achievements** | Ачивки, которые обновляются после диалога |
| **On Level Complete Achievements** | Ачивки, которые обновляются при завершении уровня |
| **On Chest Opened Achievements** | Ачивки, которые обновляются при открытии сундука |

```
┌─────────────────────────────────────────────────┐
│ Achievement Listener                            │
├─────────────────────────────────────────────────┤
│ On Kill Achievements:                           │
│   [0] → kill_10_enemies (AchievementType)       │
│   [1] → kill_50_enemies (AchievementType)       │
│   Size: 2                                       │
├─────────────────────────────────────────────────┤
│ On Collect Achievements:                        │
│   [0] → collect_20_coins (AchievementType)      │
│   Size: 1                                       │
├─────────────────────────────────────────────────┤
│ On Damage Dealt Achievements:                   │
│   Size: 0                                       │
├─────────────────────────────────────────────────┤
│ ... (остальные секции)                          │
└─────────────────────────────────────────────────┘
```

### Шаг 4: Вызывайте события из игровой логики

```csharp
using AchievementSystem;

public class EnemyHealth : MonoBehaviour
{
    public void Die()
    {
        // Убиваем врага
        Destroy(gameObject);
        
        // Сообщаем систему о достижении
        AchievementEvents.TriggerKill();
    }
}

public class PlayerCombat : MonoBehaviour
{
    public void DealDamage(float damage)
    {
        // Наносим урон
        target.TakeDamage(damage);
        
        // Сообщаем систему о достижении
        AchievementEvents.TriggerDamageDealt(damage);
    }
}
```

## Полный список событий

| Метод | Описание | Где вызывать |
|---|---|---|
| `TriggerKill()` | Враг убит | EnemyHealth.Die() |
| `TriggerCollect(Collectible)` | Предмет подобран | Player.TakeItem() |
| `TriggerDamageDealt(float)` | Нанесен урон | PlayerCombat.DealDamage() |
| `TriggerHealed(int)` | Игрок вылечен | PlayerHealthSystem.Heal() |
| `TriggerDistanceTraveled(float)` | Пройдена дистанция | Player.Update() |
| `TriggerDialogueFinished(string)` | Диалог завершен | DialogueSystem.FinishDialogue() |
| `TriggerLevelComplete(string)` | Уровень пройден | LevelManager.CompleteLevel() |
| `TriggerChestOpened(string)` | Сундук открыт | Chest.Open() |

## Программное управление ачивками

> **ВАЖНО**: Все операции выполняются через `AchievementSystemCore.Instance` — единственный синглтон системы.

```csharp
using AchievementSystem;

// Обновить прогресс ачивки
AchievementSystemCore.Instance.UpdateProgress("kill_10_enemies", 1);

// Мгновенно разблокировать ачивку
AchievementSystemCore.Instance.UnlockAchievement("complete_level_1");

// Проверить статус
bool isUnlocked = AchievementSystemCore.Instance.IsAchievementUnlocked("kill_10_enemies");

// Получить текущий прогресс
int progress = AchievementSystemCore.Instance.GetAchievementProgress("kill_10_enemies");

// Получить общее количество разблокированных
int unlockedCount = AchievementSystemCore.Instance.GetTotalUnlockedCount();

// Получить общее количество всех ачивок
int totalCount = AchievementSystemCore.Instance.GetTotalAchievementCount();

// Показать уведомление вручную
AchievementSystemCore.Instance.ShowNotification(achievementType);

// Сохранить/загрузить прогресс
AchievementSystemCore.Instance.SaveProgress();
AchievementSystemCore.Instance.LoadProgress();

// Сбросить весь прогресс
AchievementSystemCore.Instance.ResetProgress();

// Прямой доступ к AchievementManager (для продвинутых сценариев)
var manager = AchievementSystemCore.Instance.AchievementManager;
manager.AddListener(someListener);
manager.GetAllAchievementData();
```

## Интеграция с существующими системами

### С CollectibleSystem

```csharp
// В Player.cs или CollectibleManager.cs
public void Collect(Collectible collectible)
{
    CollectibleManager.Instance.Collect(collectible);
    
    // Сообщаем о подборе предмета
    AchievementEvents.TriggerCollect(collectible);
}
```

### С GameEvents

```csharp
// Подписываемся на события GameEvents
private void OnEnable()
{
    GameEvents.OnFightFinished += OnFightFinished;
    GameEvents.OnWin += OnWin;
}

private void OnFightFinished()
{
    AchievementEvents.TriggerKill();
}

private void OnWin()
{
    // Используем AchievementSystemCore.Instance вместо AchievementManager.Instance
    AchievementSystemCore.Instance.UnlockAchievement("defeat_angel");
}
```

## Advanced Patterns

### Паттерн 1: Множественные ачивки на одно событие

```
AchievementEventListener (на сцене)
  └─ On Kill Achievements
       ├─ [0] → kill_1_enemy (AchievementType)
       ├─ [1] → kill_50_enemies (AchievementType)
       └─ [2] → boss_slayer (AchievementType)
```

### Паттерн 2: Условные ачивки

```csharp
// Для сложных условий используйте код:
public void OnBossKilled()
{
    AchievementEvents.TriggerKill(); // Обычные ачивки на убийство
    
    // Специальная ачивка только для босса
    AchievementSystemCore.Instance.UnlockAchievement("defeat_blue_angel");
}
```

### Паттерн 3: Ачивки на дистанцию

```csharp
// В Player.cs Update()
private void Update()
{
    float movement = Mathf.Abs(transform.position.x - prevPosition.x);
    prevPosition = transform.position;
    
    if (movement > 0.1f)
    {
        AchievementEvents.TriggerDistanceTraveled(movement);
    }
}
```

### Паттерн 4: Ачивки с кастомным прогрессом

```csharp
// Если нужно добавить больше чем 1 к прогрессу
public void OnDamageDealt(float damage)
{
    AchievementEvents.TriggerDamageDealt(damage);
    
    // Кастомная ачивка: нанести 100 урона за бой
    if (damage >= 50f)
    {
        AchievementSystemCore.Instance.UpdateProgress("big_hit", 1);
    }
}
```

## UI компоненты

### AchievementPanelUI

Отображает полный список достижений с прогрессом.

**Настройка:**
1. Создайте UI Canvas в сцене
2. Добавьте GameObject и назовите его `AchievementPanel`
3. Добавьте компонент `AchievementPanelUI`
4. Назначьте:
   - **Slot Prefab**: префаб AchievementSlotUI
   - **Slots Container**: Transform для размещения слотов
   - **Total Count Text**: TextMeshPro для отображения общего количества
   - **Unlocked Count Text**: TextMeshPro для отображения разблокированных
   - **Close Button**: Button для закрытия панели

### AchievementSlotUI

Отображает одну ачивку с иконкой, названием, описанием и прогрессом.

**Состояния:**
- **Hidden**: ачивка скрыта (isHidden = true), показывает "???"
- **Locked**: ачивка не разблокирована, показывает прогресс
- **Unlocked**: ачивка разблокирована, показывает "Completed!"

### AchievementNotificationUI

Показывает всплывающее уведомление при разблокировке ачивки.

**Настройка:**
1. Назначьте префаб `achiev_notification.prefab` в поле **Notification Prefab** AchievementSystemCore
2. Или используйте вручную:
```csharp
AchievementSystemCore.Instance.ShowNotification(achievementType);
```

**Настройка позиции уведомления:**

Позиция уведомления на экране настраивается через `RectTransform` префаба `achiev_notification.prefab` в Unity Editor.

**Рекомендуемые настройки для верхнего центра:**
```
1. Откройте префаб achiev_notification.prefab
2. Выберите корневой объект (сам префаб)
3. В Inspector найдите RectTransform
4. Установите Anchor Preset: Upper Center (📎 зажать и перетащить)
5. Настройки:
   - Anchors: (0.5, 1) → (0.5, 1)
   - Position X: 0
   - Position Y: -50 (отступ сверху)
   - Width: 400-600
   - Height: 80-120
```

**Или для верхнего правого угла:**
```
1. Anchor Preset: Upper Right
2. Anchors: (1, 1) → (1, 1)
3. Position X: -50 (отступ справа)
4. Position Y: -50 (отступ сверху)
```

### Persistent Notification Canvas

Автоматически создаётся при инициализации AchievementSystemCore. Не требует ручной настройки.

**Параметры (в AchievementSystemCore Inspector):**
- **Auto Create Persistent Canvas**: `true` — автоматически создать Canvas
- **Canvas Resolution**: `(1920, 1080)` — базовое разрешение для масштабирования
- **Canvas Match Width/Height**: `true` — масштабирование по ширине или высоте

**Как это работает:**
1. При первом запуске создаётся GameObject `AchievementNotificationCanvas`
2. Ему добавляется Canvas с `RenderMode.ScreenSpaceCamera`
3. Добавляется CanvasScaler для адаптации к любому разрешению
4. Создаётся пустой GameObject `NotificationRoot` как родитель для уведомлений
5. Всё помечается с `DontDestroyOnLoad` — работает между сценами

## File Structure

```
Assets/TestFeatures/AchievementSystem/
├── AchievementType.cs              # ScriptableObject для ачивок
├── AchievementData.cs              # Данные прогресса ачивки
├── IAchievementListener.cs         # Интерфейс для UI
├── AchievementManager.cs           # Менеджер прогресса (НЕ Singleton)
├── AchievementEvents.cs            # Статический Event Bus
├── AchievementEventListener.cs     # Слушатель событий
├── AchievementSystemCore.cs        # ЕДИНСТВЕННЫЙ Singleton (точка входа)
├── UI/
│   ├── AchievementSlotUI.cs        # Слот ачивки
│   ├── AchievementPanelUI.cs       # Панель ачивок
│   └── AchievementNotificationUI.cs # Уведомление
├── kill_1_enemy.asset              # Пример ачивки
├── finish_1_level.asset            # Пример ачивки
├── achiev_notification.prefab      # Префаб уведомления
└── README_AchievementCondition.md  # Эта документация
```

## Troubleshooting

### Ачивка не разблокировывается

1. Убедитесь, что `AchievementEventListener` добавлен на сцену
2. Проверьте, что событие вызывается в нужном месте кода
3. Убедитесь, что AchievementType правильно настроен (ID совпадает)
4. Проверьте консоль Unity на наличие ошибок

### AchievementType пустой в Inspector

1. Убедитесь, что ScriptableObject создан правильно (Right-click → Create → Achievement System → Achievement)
2. Проверьте, что все поля заполнены (особенно ID)
3. Нажмите `Ctrl+S` чтобы сохранить изменения в ScriptableObject

### Система не инициализирована

1. Убедитесь, что AchievementSystemCore создан и находится в первой сцене
2. Проверьте, что все AchievementType назначены в поле All Achievements

## Типичные ошибки

### Ошибка: "AchievementSystemCore not found"

**Причина:** AchievementSystemCore не был создан или не инициализировался.

**Решение:** Создайте AchievementSystemCore в первой загружаемой сцене (MainMenu).

### Ошибка: "No achievements assigned"

**Причина:** В AchievementSystemCore не назначены AchievementType.

**Решение:** Перетащите AchievementType ScriptableObject в поле All Achievements в Inspector.

### Ошибка: "Notification prefab not assigned"

**Причина:** Achievement разблокируется, но уведомление не показывается.

**Решение:** Назначьте AchievementNotificationUI prefab в поле Notification Prefab AchievementSystemCore, или игнорируйте если уведомления не нужны.

### Ошибка: "Persistent notification canvas root not available"

**Причина:** `autoCreatePersistentCanvas` выключен, но `notificationParent` не назначен.

**Решение:** В AchievementSystemCore включите `Auto Create Persistent Canvas: true` или назначьте `notificationParent` (устаревший подход).

### Проблема: Уведомления исчезают при смене сцены

**Причина:** (Устаревшая) Раньше использовался `notificationParent`, который мог исчезать при смене сцены.

**Решение:** Новая архитектура использует Persistent Canvas с `DontDestroyOnLoad`. Уведомления теперь всегда появляются на экране, независимо от текущей сцены.

### Ошибка: Использование AchievementManager.Instance

**Причина:** В новой архитектуре AchievementManager больше НЕ является синглтоном.

**Решение:** Используйте `AchievementSystemCore.Instance.UpdateProgress()` вместо `AchievementManager.Instance.UpdateProgress()`. Для прямого доступа к AchievementManager используйте `AchievementSystemCore.Instance.AchievementManager`.