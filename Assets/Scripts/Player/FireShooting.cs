using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShooting : Shooting
{
    protected override void Shoot()
    {
        if (aspect.ShowToggle() == true  && aspect.isActiveAndEnabled)
        {
            animator.SetTrigger("Shoot");
            pool.SpawnFromPool("Fire", _projectileSpawnPoint.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("FireAttack");
        }  
        else if (aspect.isActiveAndEnabled == false)
        {
            animator.SetTrigger("Shoot");
            pool.SpawnFromPool("Fire", _projectileSpawnPoint.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("FireAttack");
        }
    }
}
