using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : BaseAIBehaviour
{
    public EnemyShooting _attack;
    private void Update()
    {
        MovementY();
        FireCalculate();
    }

    private void FireCalculate()
    {
        if (_attack == null) return;

        if (Time.time >= nextTimeToFire)
        {
            float currentCooldown = Random.Range(_minfireRate, _maxfireRate);

            nextTimeToFire = Time.time + currentCooldown;

            _attack.Shoot();
        }
    }
}
