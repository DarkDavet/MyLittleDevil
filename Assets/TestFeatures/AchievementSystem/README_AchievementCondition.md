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

### Шаг 2: Добавьте AchievementEventListener на сцену

1. Создайте пустой GameObject → `AchievementListeners`
2. Добавьте компонент **`AchievementEventListener`**
3. В Inspector вы увидите 8 секций для каждого типа события:

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

**В каждой секции просто перетащите AchievementType ScriptableObject из Project в поле Size:**
- Установите Size = 1 (или больше для нескольких ачивок)
- Перетащите нужные AchievementType ассеты в ячейки [0], [1], и т.д.

![Inspector Setup](#)

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

### Паттерн 4: Ачивки с кастомным прогрессом

```csharp
// Если нужно добавить больше чем 1 к прогрессу
public void OnDamageDealt(float damage)
{
    AchievementEvents.TriggerDamageDealt(damage);
    
    // Кастомная ачивка: нанести 100 урона за бой
    if (damage >= 50f)
    {
        AchievementManager.Instance.UpdateProgress("big_hit", 1);
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