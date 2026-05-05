using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCommand : ICommand
{
    private PlayerHealthSystem _healthSystem;
    private int _prevHealth;
    private int _newHealth;

    public HealthCommand(PlayerHealthSystem healthSystem, int currentHealth)
    {
        _healthSystem = healthSystem;
        // Запоминаем то, что было до записи (для Undo)
        _prevHealth = healthSystem.CurrentHealth;
        _newHealth = currentHealth;
    }

    public void Execute()
    {
        _healthSystem.CurrentHealth = _newHealth;
        GameEvents.TriggerUpdatedPlayerHealth(_newHealth);
    }

    public void Undo()
    {
        if (_prevHealth > _healthSystem.CurrentHealth)
        {
            _healthSystem.CurrentHealth = _prevHealth;
            GameEvents.TriggerUpdatedPlayerHealth(_prevHealth);

            // звук лечения?
        }
    }
}
