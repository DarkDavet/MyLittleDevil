# AchievementSystem — Usage Guide

## Новая событийно-ориентированная архитектура

Система использует **статический Event Bus** (`AchievementEvents`) для отправки событий и **AchievementEventListener** для автоматической обработки.

## Как это работает

```
[Игровая логика]          [AchievementEvents]          [AchievementEventListener]
       │                          │                              │
       │  AchievementEvents      │    AchievementEvents       │  Подписывается на
       │  .TriggerKill()        │──► OnKill event ──────────►│  события
       │  .TriggerCollect()     │──► OnCollect event ───────►│
       │  .TriggerDamageDealt() │──► OnDamageDealt event ───►│
       │                          │                              │
       │                          │                    AchievementManager
       │                          │                    UpdateProgress()
```

## Быстрый старт

### Шаг 1: Создайте AchievementType

Right-click в Project → **Create** → **Achievement System** → **Achievement**

Настройте:
- **ID**: `kill_10_enemies` (уникальный идентификатор)
- **Title**: `Monster Slayer`
- **Required Progress**: `10`
- **Progress Type**: `Counter`

### Шаг 2: Добавьте AchievementEventListener на сцену

1. Создайте пустой GameObject → `AchievementListeners`
2. Добавьте компонент **`AchievementEventListener`**
3. В Inspector настройте триггеры:

| Section | Описание |
|---|---|
| **On Kill Triggers** | Ачивки, которые обновляются при убийстве врага |
| **On Collect Triggers** | Ачивки, которые обновляются при подборе предмета |
| **On Damage Dealt Triggers** | Ачивки, которые обновляются при нанесении урона |
| **On Healed Triggers** | Ачивки, которые обновляются при лечении |
| **On Distance Traveled Triggers** | Ачивки, которые обновляются при прохождении дистанции |
| **On Dialogue Finished Triggers** | Ачивки, которые обновляются после диалога |
| **On Level Complete Triggers** | Ачивки, которые обновляются при завершении уровня |
| **On Chest Opened Triggers** | Ачивки, которые обновляются при открытии сундука |

В каждом разделе добавьте записи с **Achievement ID** и **Progress Amount**.

### Шаг 3: Вызывайте события из игровой логики

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

```csharp
using AchievementSystem;

// Обновить прогресс ачивки
AchievementManager.Instance.UpdateProgress("kill_10_enemies", 1);

// Мгновенно разблокировать ачивку
AchievementManager.Instance.UnlockAchievement("complete_level_1");

// Проверить статус
bool isUnlocked = AchievementManager.Instance.IsAchievementUnlocked("kill_10_enemies");

// Получить текущий прогресс
int progress = AchievementManager.Instance.GetAchievementProgress("kill_10_enemies");

// Получить список всех ачивок
var allAchievements = FindObjectOfType<AchievementSystem>().GetAllAchievements();
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
    AchievementManager.Instance.UnlockAchievement("defeat_angel");
}
```

## Common Patterns

### Паттерн 1: Множественные ачивки на одно событие

```
AchievementEventListener (на сцене)
  └─ On Kill Triggers
       ├─ Achievement ID: "kill_1_enemy" → Progress: 1
       ├─ Achievement ID: "kill_50_enemies" → Progress: 1
       └─ Achievement ID: "boss_slayer" → Progress: 1 (если враг — босс)
```

### Паттерн 2: Условные ачивки

```csharp
// Для сложных условий используйте код:
public void OnBossKilled()
{
    AchievementEvents.TriggerKill(); // Обычные ачивки на убийство
    
    // Специальная ачивка только для босса
    AchievementManager.Instance.UnlockAchievement("defeat_blue_angel");
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

## Удаленные компоненты

Следующие компоненты были удалены и заменены на AchievementEventListener:
- ~~CollectibleCondition~~
- ~~KillCondition~~
- ~~DistanceCondition~~
- ~~TimeCondition~~
- ~~DialogueCondition~~

Если вы использовали эти компоненты в сценах, замените их на **AchievementEventListener**.