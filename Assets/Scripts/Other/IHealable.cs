using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealable
{
    int CurrentHealth { get; set; }
    int MaxHealth { get; set; }
    void Heal(int amount);
}
