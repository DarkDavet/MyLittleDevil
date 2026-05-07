using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shooting: MonoBehaviour
{
    [SerializeField] protected Player _player;
    [SerializeField] protected GameObject _projectilePrefab;
    [SerializeField] protected Transform _projectileSpawnPoint;
    [SerializeField] protected float _fireRate = 1f;
    protected Animator animator;

    protected float nextTimeToShoot = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public abstract void Shoot();
}
