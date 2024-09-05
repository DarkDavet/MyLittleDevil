using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShooting : Shooting
{
    protected override void Shoot()
    {
        if (aspect.ShowToggle() == false)
        {
            animator.SetTrigger("IceShoot");
            pool.SpawnFromPool("Ice", _projectileSpawnPoint.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("IceAttack");
        }
    }
}
